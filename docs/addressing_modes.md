# ND-100 Addressing Modes

These three bits give the addressing mode for the instruction

## Bit Structure

The addressing mode is encoded in bits 10-8 of the instruction word and consists of the following subfields:

| Bit | Name | Description |
|-----|------|-------------|
| 10 | X | Index register flag |
| 9 | I | Indirect addressing flag |
| 8 | B | Base register flag |

### Effects

- When **X** is set: Address is indexed by X register
- When **I** is set: Address is indirect
- When **B** is set: Base-relative instead of P-relative

### Syntax Effects

Setting these bits affects the assembly language syntax as follows:

- When **X=1**: Add `,X` to the prefix of the operand
- When **I=1**: Add `I` to the prefix of the operand
- When **B=1**: Add `,B` to the suffix of the operand

## Addressing Modes

| Value | Bit Pattern | Mode | Format | Example | Effective Address |
|-------|------------|------|--------|---------|------------------|
| 0 | 000 (X=0, I=0, B=0) | P relative addressing | <disp> | STA *2 | (P) + disp |
| 1 | 001 (X=0, I=0, B=1) | B-relative addressing | <disp>, B | LDA -4,B | (B) + disp |
| 2 | 010 (X=0, I=1, B=0) | P indirect addressing | I <disp> | LDA I *22 | ((P) + disp) |
| 3 | 011 (X=0, I=1, B=1) | B indirect addressing | I <disp> ,B | JPL I 3,B | ((B) + disp) |
| 4 | 100 (X=1, I=0, B=0) | X relative addressing | <disp>,X | LDA 0,X | (X) + disp |
| 5 | 101 (X=1, I=0, B=1) | B indexed addressing | <disp>,B,X | LDA 17,B ,X | (B) + disp + (X) |
| 6 | 110 (X=1, I=1, B=0) | P indirect indexed addressing | ,X I <disp> | LDA ,X I *4 | ((P) + disp) + (X) |
| 7 | 111 (X=1, I=1, B=1) | B indirect indexed addressing | ,X I ,B <disp> | LDA ,X I ,B *4 | ((B) + disp) + (X) |

## Addressing Mode Diagrams

### P relative addressing (Value: 0, Pattern: 000)

The address is the program counter plus the displacement

**Assembly Format:** `<disp>`

**Example:** `STA *2`

**Effective Address Calculation:** `(P) + disp`

**Bit Pattern:**

```
Bit:   | 10 | 9  | 8  |
Field: | X  | I  | B  |
Value: | 0  | 0  | 0  |
```

### B-relative addressing (Value: 1, Pattern: 001)

The address is the base register plus the displacement

**Assembly Format:** `<disp>, B`

**Example:** `LDA -4,B`

**Effective Address Calculation:** `(B) + disp`

**Bit Pattern:**

```
Bit:   | 10 | 9  | 8  |
Field: | X  | I  | B  |
Value: | 0  | 0  | 1  |
```

### P indirect addressing (Value: 2, Pattern: 010)

The address is the program counter plus the displacement, and the address is indirect

**Assembly Format:** `I <disp>`

**Example:** `LDA I *22`

**Effective Address Calculation:** `((P) + disp)`

**Bit Pattern:**

```
Bit:   | 10 | 9  | 8  |
Field: | X  | I  | B  |
Value: | 0  | 1  | 0  |
```

### B indirect addressing (Value: 3, Pattern: 011)

The address is the base register plus the displacement, and the address is indirect

**Assembly Format:** `I <disp> ,B`

**Example:** `JPL I 3,B`

**Effective Address Calculation:** `((B) + disp)`

**Bit Pattern:**

```
Bit:   | 10 | 9  | 8  |
Field: | X  | I  | B  |
Value: | 0  | 1  | 1  |
```

### X relative addressing (Value: 4, Pattern: 100)

The address is the index register plus the displacement

**Assembly Format:** `<disp>,X`

**Example:** `LDA 0,X`

**Effective Address Calculation:** `(X) + disp`

**Bit Pattern:**

```
Bit:   | 10 | 9  | 8  |
Field: | X  | I  | B  |
Value: | 1  | 0  | 0  |
```

### B indexed addressing (Value: 5, Pattern: 101)

The address is the base register plus the displacement, and the address is indexed by the index register

**Assembly Format:** `<disp>,B,X`

**Example:** `LDA 17,B ,X`

**Effective Address Calculation:** `(B) + disp + (X)`

**Bit Pattern:**

```
Bit:   | 10 | 9  | 8  |
Field: | X  | I  | B  |
Value: | 1  | 0  | 1  |
```

### P indirect indexed addressing (Value: 6, Pattern: 110)

The address is the program counter plus the displacement, and the address is indirect, and the address is indexed by the index register

**Assembly Format:** `,X I <disp>`

**Example:** `LDA ,X I *4`

**Effective Address Calculation:** `((P) + disp) + (X)`

**Bit Pattern:**

```
Bit:   | 10 | 9  | 8  |
Field: | X  | I  | B  |
Value: | 1  | 1  | 0  |
```

### B indirect indexed addressing (Value: 7, Pattern: 111)

The address is the base register plus the displacement, and the address is indirect, and the address is indexed by the index register

**Assembly Format:** `,X I ,B <disp>`

**Example:** `LDA ,X I ,B *4`

**Effective Address Calculation:** `((B) + disp) + (X)`

**Bit Pattern:**

```
Bit:   | 10 | 9  | 8  |
Field: | X  | I  | B  |
Value: | 1  | 1  | 1  |
```

