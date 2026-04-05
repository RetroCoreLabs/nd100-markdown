using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NDGen.Core.Models;

namespace NDGen.Core.Utils
{
    public class MarkdownGenerator
    {
        private readonly string _outputPath;
        private readonly DefinitionLoader _loader;

        public MarkdownGenerator(string outputPath, DefinitionLoader loader)
        {
            _outputPath = outputPath;
            _loader = loader;
        }

        public void Generate()
        {
            var instructions = _loader.GetInstructions();
            var cpuDefinition = _loader.GetCpuDefinition();

            // Create output directory if it doesn't exist
            Directory.CreateDirectory(_outputPath);

            // Generate CPU documentation
            GenerateCpuDocumentation(cpuDefinition, instructions);

            // Generate index file
            GenerateIndex(instructions);
        }

        private void GenerateIndex(IReadOnlyList<InstructionDefinition> instructions)
        {
            var sb = new StringBuilder();
            sb.AppendLine("# ND-100 Instruction Set Reference");
            sb.AppendLine();
            sb.AppendLine("## Table of Contents");
            sb.AppendLine();

            // Group instructions by category
            var groupedInstructions = instructions
                .GroupBy(i => i.Category)
                .OrderBy(g => g.Key);

            foreach (var group in groupedInstructions)
            {
                sb.AppendLine($"### {group.Key}");
                sb.AppendLine();
                foreach (var instruction in group.OrderBy(i => i.Name))
                {
                    sb.AppendLine($"- [{instruction.Name}](cpu_documentation.md#{instruction.Name.ToLower()}) - {instruction.Description.Split('\n')[0]}");
                }
                sb.AppendLine();
            }

            File.WriteAllText(Path.Combine(_outputPath, "index.md"), sb.ToString());
        }

        /// <summary>
        /// Generates detailed documentation for addressing modes, including the subfield structure
        /// </summary>
        private void GenerateAddressingModesDocumentation()
        {
            var addressingModeType = _loader.GetCpuDefinition().Metadata.OperandTypes
                .FirstOrDefault(ot => ot.Type == "addressing_mode");
                
            if (addressingModeType == null)
                return;
                
            var sb = new StringBuilder();
            
            sb.AppendLine("# ND-100 Addressing Modes");
            sb.AppendLine();
            sb.AppendLine(addressingModeType.Description);
            sb.AppendLine();
            
            // Generate subfields documentation
            if (addressingModeType.Subfields != null && addressingModeType.Subfields.Any())
            {
                sb.AppendLine("## Bit Structure");
                sb.AppendLine();
                sb.AppendLine("The addressing mode is encoded in bits 10-8 of the instruction word and consists of the following subfields:");
                sb.AppendLine();
                
                sb.AppendLine("| Bit | Name | Description |");
                sb.AppendLine("|-----|------|-------------|");
                
                foreach (var subfield in addressingModeType.Subfields.OrderByDescending(sf => sf.Position))
                {
                    sb.AppendLine($"| {subfield.Position} | {subfield.Name} | {subfield.Description} |");
                }
                sb.AppendLine();
                
                // Documentation about effects when bits are set
                sb.AppendLine("### Effects");
                sb.AppendLine();
                
                foreach (var subfield in addressingModeType.Subfields)
                {
                    if (!string.IsNullOrEmpty(subfield.EffectWhenSet))
                    {
                        sb.AppendLine($"- When **{subfield.Name}** is set: {subfield.EffectWhenSet}");
                    }
                }
                sb.AppendLine();
                
                // Composite effects on assembly syntax
                if (addressingModeType.CompositeEffects != null && addressingModeType.CompositeEffects.Any())
                {
                    sb.AppendLine("### Syntax Effects");
                    sb.AppendLine();
                    sb.AppendLine("Setting these bits affects the assembly language syntax as follows:");
                    sb.AppendLine();
                    
                    foreach (var effect in addressingModeType.CompositeEffects)
                    {
                        sb.AppendLine($"- When **{effect.Condition}**: Add `{effect.FormatAddition}` to the {effect.Position} of the operand");
                    }
                    sb.AppendLine();
                }
            }
            
            // Generate addressing modes table
            if (addressingModeType.Values != null && addressingModeType.Values.Any())
            {
                sb.AppendLine("## Addressing Modes");
                sb.AppendLine();
                
                sb.AppendLine("| Value | Bit Pattern | Mode | Format | Example | Effective Address |");
                sb.AppendLine("|-------|------------|------|--------|---------|------------------|");
                
                foreach (var mode in addressingModeType.Values.OrderBy(v => v.Value))
                {
                    string bitPattern = mode.BitPattern ?? "";
                    string xib = ""; 
                    
                    if (mode.Fields != null)
                    {
                        xib = $"X={GetFieldValue(mode.Fields, "X")}, " +
                              $"I={GetFieldValue(mode.Fields, "I")}, " +
                              $"B={GetFieldValue(mode.Fields, "B")}";
                    }
                    
                    sb.AppendLine($"| {mode.Value} | {bitPattern} ({xib}) | {mode.Mode} | {mode.Format} | {mode.Example} | {mode.EffectiveAddress} |");
                }
                sb.AppendLine();
                
                // Generate diagrams for each addressing mode
                sb.AppendLine("## Addressing Mode Diagrams");
                sb.AppendLine();
                
                foreach (var mode in addressingModeType.Values)
                {
                    sb.AppendLine($"### {mode.Mode} (Value: {mode.Value}, Pattern: {mode.BitPattern})");
                    sb.AppendLine();
                    sb.AppendLine(mode.Description);
                    sb.AppendLine();
                    
                    // Create a sample instruction format
                    sb.AppendLine("**Assembly Format:** `" + mode.Format + "`");
                    sb.AppendLine();
                    sb.AppendLine("**Example:** `" + mode.Example + "`");
                    sb.AppendLine();
                    sb.AppendLine("**Effective Address Calculation:** `" + mode.EffectiveAddress + "`");
                    sb.AppendLine();
                    
                    // Create a bit field diagram for this mode
                    if (mode.BitPattern?.Length == 3 && addressingModeType.Subfields?.Count == 3)
                    {
                        sb.AppendLine("**Bit Pattern:**");
                        sb.AppendLine();
                        sb.AppendLine("```");
                        sb.AppendLine($"Bit:   | 10 | 9  | 8  |");
                        sb.AppendLine($"Field: | X  | I  | B  |");
                        sb.AppendLine($"Value: | {mode.BitPattern[0]}  | {mode.BitPattern[1]}  | {mode.BitPattern[2]}  |");
                        sb.AppendLine("```");
                        sb.AppendLine();
                    }
                }
            }
            
            // Write the addressing modes documentation to file
            File.WriteAllText(Path.Combine(_outputPath, "addressing_modes.md"), sb.ToString());
        }

        /// <summary>
        /// Gets the value of a field from the fields collection
        /// </summary>
        private string GetFieldValue(List<Dictionary<string, object>> fields, string fieldName)
        {
            foreach (var field in fields)
            {
                if (field.TryGetValue(fieldName, out var value))
                {
                    return value?.ToString() ?? "?";
                }
            }
            return "?";
        }

        private void GenerateCpuDocumentation(CpuDefinition cpuDefinition, IReadOnlyList<InstructionDefinition> instructions)
        {
            var sb = new StringBuilder();
            
            // Title with decorative header
            sb.AppendLine("================================================================================");
            sb.AppendLine("#                              CPU Documentation                                #");
            sb.AppendLine("================================================================================");
            sb.AppendLine();

            // Table of Contents with visual hierarchy
            sb.AppendLine("## 📑 Table of Contents");
            sb.AppendLine();
            sb.AppendLine("### 🔍 CPU Information");
            sb.AppendLine("- [CPU Overview](#cpu-overview)");
            sb.AppendLine("- [Architecture](#architecture)");
            sb.AppendLine();
            sb.AppendLine("### ⚙️ Registers");
            sb.AppendLine("- [General Purpose Registers](#general-purpose-registers)");
            sb.AppendLine("- [Special Registers](#special-registers)");
            sb.AppendLine();
            sb.AppendLine("### 📚 Instruction Classes");
            foreach (var instructionClass in cpuDefinition.Metadata.InstructionClasses)
            {
                sb.AppendLine($"- [{instructionClass.Name}](#{instructionClass.Name.ToLower()})");
            }
            sb.AppendLine();
            sb.AppendLine("### 🏠 Addressing Modes");
            sb.AppendLine("- [Addressing Modes](#addressing-modes)");
            sb.AppendLine();
            sb.AppendLine("### 💻 Instructions by Category");
            foreach (var group in instructions.GroupBy(i => i.Category).OrderBy(g => g.Key))
            {
                sb.AppendLine($"- [{group.Key}](#{group.Key.ToLower().Replace(" ", "-")})");
            }
            sb.AppendLine();
            sb.AppendLine("### 📖 Instruction Reference");
            foreach (var instruction in instructions.OrderBy(i => i.Name))
            {
                //sb.AppendLine($"- [{instruction.Name}](#{instruction.Name.ToLower()})");
                sb.AppendLine($"- [{(instruction.OpcodeValueFormatted)}] [{instruction.Name}](#{instruction.Name.ToLower()})");
            }
            sb.AppendLine();

            // CPU Information section
            sb.AppendLine("================================================================================");
            sb.AppendLine("## 🔍 CPU Information");
            sb.AppendLine("================================================================================");
            sb.AppendLine();
            sb.AppendLine($"### {cpuDefinition.Metadata.CpuName}");
            sb.AppendLine();
            sb.AppendLine($"**Word Size:** {cpuDefinition.Metadata.WordSize} bits");
            sb.AppendLine($"**Endianness:** {cpuDefinition.Metadata.Endianness}");
            sb.AppendLine();

            // Registers section
            sb.AppendLine("================================================================================");
            sb.AppendLine("## ⚙️ Registers");
            sb.AppendLine("================================================================================");
            sb.AppendLine();
            sb.AppendLine("| Name | Description | Size | Type |");
            sb.AppendLine("|:-----|:------------|:-----|:-----|");
            foreach (var register in cpuDefinition.Metadata.Registers)
            {
                sb.AppendLine($"| `{register.Name}` | {register.Description} | {register.Size} bits | {register.Type} |");
            }
            sb.AppendLine();

            // Instruction Classes section
            sb.AppendLine("================================================================================");
            sb.AppendLine("## 📚 Instruction Classes");
            sb.AppendLine("================================================================================");
            sb.AppendLine();
            foreach (var instructionClass in cpuDefinition.Metadata.InstructionClasses)
            {
                sb.AppendLine($"### {instructionClass.Name}");
                sb.AppendLine();
                sb.AppendLine(instructionClass.Description);
                sb.AppendLine();
                sb.AppendLine($"**Format:** `{instructionClass.Format}`");
                sb.AppendLine($"**Mask:** `{instructionClass.Mask}`");
                sb.AppendLine();

                if (!string.IsNullOrEmpty(instructionClass.GeneratedBitFormat))
                {
                    sb.AppendLine("**Bit Format:**");
                    sb.AppendLine("```");
                    sb.AppendLine(instructionClass.GeneratedBitFormat);
                    sb.AppendLine("```");
                }
                sb.AppendLine("--------------------------------------------------------------------------------");
                sb.AppendLine();
            }

            // Addressing Modes section
            GenerateAddressingModesSection(sb, cpuDefinition);

            // Instructions by Category section
            sb.AppendLine("================================================================================");
            sb.AppendLine("## 💻 Instructions by Category");
            sb.AppendLine("================================================================================");
            sb.AppendLine();

            var groupedInstructions = instructions
                .GroupBy(i => i.Category)
                .OrderBy(g => g.Key);

            foreach (var group in groupedInstructions)
            {
                sb.AppendLine($"### {group.Key}");
                sb.AppendLine();
                sb.AppendLine("| Instruction | Description |");
                sb.AppendLine("|:------------|:------------|");
                foreach (var instruction in group.OrderBy(i => i.Name))
                {
                    var shortDesc = instruction.Description.Split('\n')[0];
                    sb.AppendLine($"| [`{instruction.Name}`](#{instruction.Name.ToLower()}) | {shortDesc} |");
                }
                sb.AppendLine();
                // Insert category documentation if available
                var lookupKey = group.Key.ToLowerInvariant().Replace(" ", " ");
                if (_loader.CategoryDocs.TryGetValue(lookupKey, out var mdContent))
                {
                    sb.AppendLine(mdContent); // Insert as-is
                    // TODO: Add code here to adjust formatting of category markdown if needed in the future
                }
                sb.AppendLine("--------------------------------------------------------------------------------");
                sb.AppendLine();
            }

            // Full Instruction Reference section
            sb.AppendLine("================================================================================");
            sb.AppendLine("## 📖 Instruction Reference");
            sb.AppendLine("================================================================================");
            sb.AppendLine();

            foreach (var instruction in instructions.OrderBy(i => i.Name))
            {
                // Instruction header
                sb.AppendLine($"### {instruction.Name}");
                sb.AppendLine();

                // Quick Reference
                sb.AppendLine("#### ⚡ Quick Reference");
                sb.AppendLine();
                sb.AppendLine("| Property | Value |");
                sb.AppendLine("|:---------|:-------|");
                sb.AppendLine($"| Opcode | `{instruction.OpcodeValueFormatted}` (octal) |");
                sb.AppendLine($"| Format | `{instruction.Format}` |");
                sb.AppendLine($"| Category | {instruction.Category} |");
                sb.AppendLine($"| Privilege | {instruction.Privilege} |");
                if (!string.IsNullOrEmpty(instruction.Mask))
                {
                    sb.AppendLine($"| Mask | `{instruction.Mask}` |");
                }
                sb.AppendLine();

                // Description
                sb.AppendLine("#### 📝 Description");
                sb.AppendLine();
                sb.AppendLine(ProcessSpecialTags(instruction.Description));
                sb.AppendLine();

                // Detailed Description (if available)
                if (!string.IsNullOrWhiteSpace(instruction.DetailedDescription))
                {
                    sb.AppendLine("#### 📚 Detailed Information");
                    sb.AppendLine();
                    sb.AppendLine(ProcessSpecialTags(instruction.DetailedDescription));
                    sb.AppendLine();
                }

                // Format
                sb.AppendLine("#### 📋 Format");
                sb.AppendLine();
                sb.AppendLine("```");
                sb.AppendLine(instruction.Format);
                sb.AppendLine("```");
                sb.AppendLine();

                // Bit Format
                if (!string.IsNullOrEmpty(instruction.GeneratedBitFormat))
                {
                    sb.AppendLine("#### Bit Layout");
                    sb.AppendLine();
                    sb.AppendLine("```");
                    sb.AppendLine(instruction.GeneratedBitFormat);
                    sb.AppendLine("```");
                    sb.AppendLine();
                }

                // Add addressing mode note for memory transfer instructions
                if (instruction.InstructionClass == "memory_transfer")
                {
                    sb.AppendLine("> **Note:** This instruction uses addressing modes. See [Addressing Modes](#addressing-modes) for details.");
                    sb.AppendLine();
                }

                // Operands
                var operands = instruction.Operands ?? new List<InstructionOperand>();
                if (operands.Any())
                {
                    sb.AppendLine("#### 🔧 Operands");
                    sb.AppendLine();
                    sb.AppendLine("| Name | Type | Bits | Description |");
                    sb.AppendLine("|:-----|:-----|:-----|:------------|");
                    foreach (var operand in operands)
                    {
                        var type = operand.Type ?? "N/A";
                        var bits = operand.Bits ?? "N/A";
                        var description = operand.Description.Replace("\n", "<br>");

                        // All enum values will be shown inline (regardless of count)
                        if (operand.Enum != null && operand.Enum.Any())
                        {
                            description += "<br><br>**Values:**<br>";
                            foreach (var enumValue in operand.Enum)
                            {
                                description += $"- `{enumValue.Name}` (`{enumValue.Value}`): {enumValue.Description}<br>";
                            }
                        }

                        sb.AppendLine($"| `{operand.Name}` | {type} | {bits} | {description} |");
                    }
                    sb.AppendLine();
                }

                // Flags
                if (instruction.FlagsAffected != null && instruction.FlagsAffected.Any())
                {
                    sb.AppendLine("#### 🚩 Flags Affected");
                    sb.AppendLine();
                    sb.AppendLine("| Flag | Description |");
                    sb.AppendLine("|:-----|:------------|");

                    var stsRegister = cpuDefinition.Metadata.Registers.FirstOrDefault(r => r.Name == "STS");
                    var flagBits = stsRegister?.Bits?.Where(b => b.Size == 1).ToDictionary(b => b.Name, b => b.Description) 
                        ?? new Dictionary<string, string>();

                    foreach (var flag in instruction.FlagsAffected)
                    {
                        if (flagBits.TryGetValue(flag, out var description))
                        {
                            sb.AppendLine($"| `{flag}` | {description} |");
                        }
                    }
                    sb.AppendLine();
                }

                // Examples
                if (instruction.Examples != null && instruction.Examples.Any())
                {
                    sb.AppendLine("#### 📚 Examples");
                    sb.AppendLine();
                    foreach (var example in instruction.Examples)
                    {
                        sb.AppendLine($"##### {example.Description}");
                        sb.AppendLine("```");
                        sb.AppendLine(example.Code);
                        sb.AppendLine("```");
                        sb.AppendLine();
                    }
                }

                // Separator between instructions
                sb.AppendLine("--------------------------------------------------------------------------------");
                sb.AppendLine();
            }

            // Write the CPU documentation file
            File.WriteAllText(Path.Combine(_outputPath, "cpu_documentation.md"), sb.ToString());
        }

        /// <summary>
        /// Process special formatting tags in text, such as ASCII tables
        /// </summary>
        private string ProcessSpecialTags(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            // Process ASCII tables
            if (text.Contains("<ascii-table>") && text.Contains("</ascii-table>"))
            {
                var regex = new System.Text.RegularExpressions.Regex("<ascii-table>(.*?)</ascii-table>", 
                    System.Text.RegularExpressions.RegexOptions.Singleline);
                
                return regex.Replace(text, match => 
                {
                    var tableContent = match.Groups[1].Value;
                    return "```\n" + tableContent + "\n```";
                });
            }

            return text;
        }

        /// <summary>
        /// Generates the addressing modes section for CPU documentation
        /// </summary>
        private void GenerateAddressingModesSection(StringBuilder sb, CpuDefinition cpuDefinition)
        {
            sb.AppendLine("================================================================================");
            sb.AppendLine("## 🏠 Addressing Modes");
            sb.AppendLine("================================================================================");
            sb.AppendLine();

            var addressingModeType = cpuDefinition.Metadata.OperandTypes
                .FirstOrDefault(ot => ot.Type == "addressing_mode");

            if (addressingModeType == null)
            {
                sb.AppendLine("No addressing modes defined.");
                sb.AppendLine();
                return;
            }

            sb.AppendLine(addressingModeType.Description);
            sb.AppendLine();

            // Generate subfields documentation if available
            if (addressingModeType.Subfields != null && addressingModeType.Subfields.Any())
            {
                sb.AppendLine("### Bit Structure");
                sb.AppendLine();
                sb.AppendLine("The addressing mode is encoded in the instruction word and consists of the following subfields:");
                sb.AppendLine();
                
                sb.AppendLine("| Bit | Name | Description |");
                sb.AppendLine("|-----|------|-------------|");
                
                foreach (var subfield in addressingModeType.Subfields.OrderByDescending(sf => sf.Position))
                {
                    sb.AppendLine($"| {subfield.Position} | {subfield.Name} | {subfield.Description} |");
                }
                sb.AppendLine();

                // Document bit effects
                sb.AppendLine("#### Effects when Bits are Set");
                sb.AppendLine();
                
                foreach (var subfield in addressingModeType.Subfields)
                {
                    if (!string.IsNullOrEmpty(subfield.EffectWhenSet))
                    {
                        sb.AppendLine($"- **{subfield.Name}**: {subfield.EffectWhenSet}");
                    }
                }
                sb.AppendLine();

                // Document composite effects
                if (addressingModeType.CompositeEffects != null && addressingModeType.CompositeEffects.Any())
                {
                    sb.AppendLine("#### Syntax Variations");
                    sb.AppendLine();
                    sb.AppendLine("When multiple bits are set, the assembly syntax changes as follows:");
                    sb.AppendLine();
                    
                    foreach (var effect in addressingModeType.CompositeEffects)
                    {
                        sb.AppendLine($"- When {effect.Condition}: Add `{effect.FormatAddition}` {effect.Position} of operand");
                    }
                    sb.AppendLine();
                }
            }

            // Generate a summary table of addressing modes
            if (addressingModeType.Values != null && addressingModeType.Values.Any())
            {
                sb.AppendLine("### Summary of Addressing Modes");
                sb.AppendLine();
                
                sb.AppendLine("| Mode | Value | Format | Example | Effective Address |");
                sb.AppendLine("|------|-------|--------|---------|------------------|");
                
                foreach (var mode in addressingModeType.Values.OrderBy(v => v.Value))
                {
                    var modeValue = mode.Value.HasValue ? mode.Value.ToString() : "?";
                    var format = mode.Format ?? "?";
                    var example = mode.Example ?? "?";
                    var effectiveAddr = mode.EffectiveAddress ?? "?";
                    
                    sb.AppendLine($"| {mode.Name} | {modeValue} | `{format}` | `{example}` | {effectiveAddr} |");
                }
                sb.AppendLine();
            }

            sb.AppendLine("See [Addressing Modes](addressing_modes.md) for more detailed information.");
            sb.AppendLine();
            sb.AppendLine("--------------------------------------------------------------------------------");
            sb.AppendLine();
            
            // Also generate the detailed addressing modes documentation
            GenerateAddressingModesDocumentation();
        }
    }
}
