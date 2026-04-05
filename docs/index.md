# ND-100 Instruction Set Reference

## Table of Contents

### Argument Instruction

- [AAA](cpu_documentation.md#aaa) - Add argument to A
- [AAB](cpu_documentation.md#aab) - Add argument to B
- [AAT](cpu_documentation.md#aat) - Add argument to T
- [AAX](cpu_documentation.md#aax) - Add argument to X
- [SAA](cpu_documentation.md#saa) - Set argument to A
- [SAB](cpu_documentation.md#sab) - Set argument to B
- [SAT](cpu_documentation.md#sat) - Set argument to T
- [SAX](cpu_documentation.md#sax) - Set argument to X

### Arithmetic and Logical

- [ADD](cpu_documentation.md#add) - Add to A register
- [AND](cpu_documentation.md#and) - Logical AND to A register
- [MPY](cpu_documentation.md#mpy) - Multiply integer
- [ORA](cpu_documentation.md#ora) - Logical inclusive OR to A register
- [SUB](cpu_documentation.md#sub) - Subtract from A register

### Bit instructions

- [BANC](cpu_documentation.md#banc) - logical AND with bit complement
- [BAND](cpu_documentation.md#band) - Logical AND to K
- [BLDA](cpu_documentation.md#blda) - Load K
- [BLDC](cpu_documentation.md#bldc) - Load K and complement
- [BORA](cpu_documentation.md#bora) - Logical OR to K
- [BSET](cpu_documentation.md#bset) - Set specified bit in equal to specified condition
- [BSKP](cpu_documentation.md#bskp) - Skip next location if specified condition is true
- [BSTA](cpu_documentation.md#bsta) - Store and clear K
- [BSTC](cpu_documentation.md#bstc) - Store complement and set K to 1

### Bit instructions_with_destination

- [BORC](cpu_documentation.md#borc) - Logical OR with bit complement

### Byte Instructions

- [BFILL](cpu_documentation.md#bfill) - Byte fill.
- [LBYT](cpu_documentation.md#lbyt) - Load byte from memory to A register
- [MOVB](cpu_documentation.md#movb) - Move byte.
- [MOVBF](cpu_documentation.md#movbf) - Move bytes forward.
- [SBYT](cpu_documentation.md#sbyt) - Store byte from A register to memory

### Control Instructions

- [CHREENTPAGES](cpu_documentation.md#chreentpages) - Change page tables.
- [CLEPT](cpu_documentation.md#clept) - Clear page tables.
- [CLEPU](cpu_documentation.md#clepu) - Clear page tables and collect PGU information.
- [CLNREENT](cpu_documentation.md#clnreent) - Clear non reentrant pages.
- [CLPT](cpu_documentation.md#clpt) - Clear segment from the page tables.
- [CNREK](cpu_documentation.md#cnrek) - Clear non reentrant pages (SINTRAN K only).    
- [ENPT](cpu_documentation.md#enpt) - Enter segment in page tables. 
- [INSPL](cpu_documentation.md#inspl) - Insert page in page list. (See Appendix B for a software description.)
- [LACB](cpu_documentation.md#lacb) - Load the A register from the core map-table bank (CMBNK).
- [LASB](cpu_documentation.md#lasb) - Load the A register with the contents of the segment-table bank (STBNK).
- [LBIT](cpu_documentation.md#lbit) - Load single bit accumulator (K) with logical memory bit.
- [LBITP](cpu_documentation.md#lbitp) - Load single bit accumulator (K) with physical memory bit.
- [LBYTP](cpu_documentation.md#lbytp) - Load the A register with a byte from physical memory.
- [LXCB](cpu_documentation.md#lxcb) - Load the X register from the core table bank (CMBNK).
- [LXSB](cpu_documentation.md#lxsb) - Load the X register from the segment table bank (STBNK).
- [RDUSP](cpu_documentation.md#rdusp) - Read a physical memory word without using cache.
- [REMPL](cpu_documentation.md#rempl) - Remove page from page list.
- [REPT](cpu_documentation.md#rept) - Enter reentrant segment in page tables. (See Appendix B for a software description.)
- [RGLOB](cpu_documentation.md#rglob) - Examine global pointers.
- [SACB](cpu_documentation.md#sacb) - Store the A register in the core map table bank (CMBNK).
- [SASB](cpu_documentation.md#sasb) - Store the A register contents in the segment table bank (STBNK).
- [SBIT](cpu_documentation.md#sbit) - Store the single bit accumulator (K) in a logical memory bit.
- [SBITP](cpu_documentation.md#sbitp) - Store the single bit accumulator (K) in a physical memory bit.
- [SBYTP](cpu_documentation.md#sbytp) - Store a byte in physical memory.
- [SETPT](cpu_documentation.md#setpt) - Set page tables.
- [SZCB](cpu_documentation.md#szcb) - Store zero in the core map-table bank (CMBNK).
- [SZSB](cpu_documentation.md#szsb) - Store zero in the segment-table bank (STBNK).
- [TSETP](cpu_documentation.md#tsetp) - Test and set physical memory word.
- [WGLOB](cpu_documentation.md#wglob) - Initialize global pointers.

### Decimal Instructions

- [ADDD](cpu_documentation.md#addd) - Add two decimal operands.
- [COMD](cpu_documentation.md#comd) - Compare two decimal operands.
- [PACK](cpu_documentation.md#pack) - Convert to BCD.
- [SHDE](cpu_documentation.md#shde) - Decimal shift.
- [SUBD](cpu_documentation.md#subd) - Subtract two decimal operands.
- [UPACK](cpu_documentation.md#upack) - Convert to ASCII.

### Execute

- [EXR](cpu_documentation.md#exr) - Execute instruction found in specified register

### Floating Conversion (Standard Format)

- [DNZ](cpu_documentation.md#dnz) - Denormalise     
- [NLZ](cpu_documentation.md#nlz) - Normalize

### Input and Output

- [IOT](cpu_documentation.md#iot) - NORD-1 INSTRUCTION (DO NOT USE)
- [IOX](cpu_documentation.md#iox) - Exchange information between I/O system and A register.
- [IOXT](cpu_documentation.md#ioxt) - Exchange information between I/O system and A register.

### Inter-level Instructions

- [IRR](cpu_documentation.md#irr) - Inter Register Read
- [IRW](cpu_documentation.md#irw) - Inter Register Write

### Interrupt Control Instructions

- [IDENT](cpu_documentation.md#ident) - Transfer IDENT code of interrupting device with highest priority on the specified level to A register. 
- [IOF](cpu_documentation.md#iof) - Interrupt System OFF
- [ION](cpu_documentation.md#ion) - Interrupt System ON
- [OPCOM](cpu_documentation.md#opcom) - Operator Communication
- [POF](cpu_documentation.md#pof) - Memory management OFF
- [WAIT](cpu_documentation.md#wait) - When interrupt system off: halts the program and enters the operator's communication. 

### Memory Examine and Test Instructions

- [RDUS](cpu_documentation.md#rdus) - Read a word without using cache
- [TSET](cpu_documentation.md#tset) - Test and set

### Memory Management Instructions

- [PIOF](cpu_documentation.md#piof) - Memory management and interrupt system OFF
- [PION](cpu_documentation.md#pion) - Memory management and interrupt system ON
- [PON](cpu_documentation.md#pon) - Memory management ON
- [REX](cpu_documentation.md#rex) - Reset extended address mode
- [SEX](cpu_documentation.md#sex) - Set extended address mode

### Memory Transfer - Double word instructions

- [LDD](cpu_documentation.md#ldd) - Load double word
- [STD](cpu_documentation.md#std) - Store double word

### Memory Transfer - Load Instruction

- [LDA](cpu_documentation.md#lda) - Load A register
- [LDT](cpu_documentation.md#ldt) - Load T register
- [LDX](cpu_documentation.md#ldx) - Load X register

### Memory Transfer - Store Instruction

- [MIN](cpu_documentation.md#min) - Memory increment and skip next instruction if zero (EA): = (EA) + 1
- [STA](cpu_documentation.md#sta) - Store A register to memory location 
- [STT](cpu_documentation.md#stt) - Store T register to memory location
- [STX](cpu_documentation.md#stx) - Store X register to memory location 
- [STZ](cpu_documentation.md#stz) - Store zero to memory location

### Memory Transfer Instructions

- [LDATX](cpu_documentation.md#ldatx) - Load A register
- [LDBTX](cpu_documentation.md#ldbtx) - Load B register
- [LDDTX](cpu_documentation.md#lddtx) - Load double word
- [LDXTX](cpu_documentation.md#ldxtx) - Load X register
- [STATX](cpu_documentation.md#statx) - Store A register
- [STDTX](cpu_documentation.md#stdtx) - Store double word
- [STZTX](cpu_documentation.md#stztx) - Store zero

### Monitor Calls

- [MON](cpu_documentation.md#mon) - The MON instruction is used in special different contexts when running under an operating system.

### Physical Memory Control Instructions

- [DEPO](cpu_documentation.md#depo) - Deposit
- [EXAM](cpu_documentation.md#exam) - Examine

### privileged

- [LWCS](cpu_documentation.md#lwcs) - Writable Control Store Instruction

### Register Block Instructions

- [LRB](cpu_documentation.md#lrb) - Load register block
- [SRB](cpu_documentation.md#srb) - Store register block

### Register Operations

- [COPY](cpu_documentation.md#copy) - Copy source to destination
- [EXIT](cpu_documentation.md#exit) - Return from subroutine
- [MIX3](cpu_documentation.md#mix3) - Multiply index by 3.
- [RADD](cpu_documentation.md#radd) - Add source to destination (dr): = (dr) + (sr)
- [RAND](cpu_documentation.md#rand) - Logical AND to destination 
- [RCLR](cpu_documentation.md#rclr) - Register clear
- [RDCR](cpu_documentation.md#rdcr) - Register decrement
- [RDIV](cpu_documentation.md#rdiv) - Divide double accumulator with source; quotient in A, remainder in D
- [REXO](cpu_documentation.md#rexo) - Logical exclusive OR 
- [RINC](cpu_documentation.md#rinc) - Register increment
- [RMPY](cpu_documentation.md#rmpy) - Multiply source with destination; result in double accumulator
- [RORA](cpu_documentation.md#rora) - OR register
- [RSUB](cpu_documentation.md#rsub) - Register subtract
- [SWAP](cpu_documentation.md#swap) - Register swap

### Register Transfer

- [MCL](cpu_documentation.md#mcl) - Masked clear
- [MST](cpu_documentation.md#mst) - Masked set
- [TRA](cpu_documentation.md#tra) - Transfer to A register
- [TRR](cpu_documentation.md#trr) - Transfer A to internal register

### Sequencing Instructions

- [JAF](cpu_documentation.md#jaf) - Condtion: Jump if (A) != 0 (jump if A filled)
- [JAN](cpu_documentation.md#jan) - Condtion: Jump if (A) < 0 (jump if A is negative)
- [JAP](cpu_documentation.md#jap) - Condtion: Jump if (A) > 0 (jump if A positive)
- [JAZ](cpu_documentation.md#jaz) - Condtion: Jump if (A) == 0 (jump if A is zero)
- [JMP](cpu_documentation.md#jmp) - Jump - Unconditional jump to specified address
- [JNC](cpu_documentation.md#jnc) - Increment X and jump if X is negative    
- [JPC](cpu_documentation.md#jpc) - Increment X and jump if X is positive    
- [JPL](cpu_documentation.md#jpl) - Jump if Plus - Jump to specified address if the result of the last operation was positive (sign bit is 0)
- [JXN](cpu_documentation.md#jxn) - Condtion: Jump if (X) < 0 (jump if X negative)
- [JXZ](cpu_documentation.md#jxz) - Condtion: Jump if (X) == 0 (jump if X is zero)

### Shift Instructions

- [SAD](cpu_documentation.md#sad) - Shift A and D registers connected
- [SHA](cpu_documentation.md#sha) - Shift A register
- [SHD](cpu_documentation.md#shd) - Shift D register
- [SHT](cpu_documentation.md#sht) - Shift T register

### Skip Instruction

- [SKP](cpu_documentation.md#skp) - The next instruction is skipped if a specified condition is true.

### Stack Operations

- [ELEAV](cpu_documentation.md#eleav) - Error leave stack.
- [ENTR](cpu_documentation.md#entr) - Enter stack.
- [INIT](cpu_documentation.md#init) - Initialize stack. 
- [LEAVE](cpu_documentation.md#leave) - Leave stack.

### Standard Floating Instructions

- [FAD](cpu_documentation.md#fad) - Add to floating point accumulator
- [FDV](cpu_documentation.md#fdv) - Divide floating point accumulator
- [FMU](cpu_documentation.md#fmu) - Multiply floating point accumulator.
- [FSB](cpu_documentation.md#fsb) - Subtract from floating point accumulator.
- [LDF](cpu_documentation.md#ldf) - Load floating accumulator (TAD) from memory (FW)
- [STF](cpu_documentation.md#stf) - Store floating accumulator (TAD) to memory (ea)

### System/CPU Information

- [VERSN](cpu_documentation.md#versn) - ** ND-110/ND-120 ONLY**

### Undocumented Instructions

- [GECO](cpu_documentation.md#geco) - GECO is a customer-specifed instruction which appears to be included as part of the standard instruction set from ND-100/CE and later. 

### Word Block Instructions

- [MOVEW](cpu_documentation.md#movew) - Word block move.

