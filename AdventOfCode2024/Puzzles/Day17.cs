namespace AdventOfCode2024.Puzzles;

public class Day17 : PuzzleBase
{
    private static void FindA(long[] registers, int steps, int[] program, long a, List<long> results)
    {
        for (var i = 0; i < 8; i++)
        {
            var newA = (a * 8) + i;
            var output = new Computer(new List<long> { newA }.Concat(registers.Skip(1)).ToArray(), program).Run();
            var expectedOutput = string.Join(",", program.Reverse().Take(steps + 1).Reverse());
            if (output == expectedOutput)
            {
                if (steps + 1 == program.Length)
                {
                    results.Add(newA);
                }
                else
                {
                    FindA(registers, steps + 1, program, newA, results);
                }
            }
        }
    }

    public override void Solve()
    {
        var registers = ReadBlockLines().Select(x => x.Split(':')[1]).Select(long.Parse).ToArray();
        var program = ReadBlockText().Replace("Program: ", "").Split(',').Select(int.Parse).ToArray();
        Console.WriteLine(new Computer(registers.ToArray(), program).Run());

        var results = new List<long>();
        FindA(registers, 0, program, 0, results);
        Console.WriteLine(results.Min());
    }

    private sealed class Computer
    {
        private readonly Dictionary<int, Func<long>> comboOperandDict;
        private readonly Dictionary<int, Action> opCodeDict;
        private readonly List<int> output = [];
        private readonly int[] program;
        private readonly long[] registers;
        private int pointer;

        public Computer(long[] registers, int[] program, int pointer = 0)
        {
            this.comboOperandDict = new Dictionary<int, Func<long>>
            {
                { 0, () => 0 },
                { 1, () => 1 },
                { 2, () => 2 },
                { 3, () => 3 },
                { 4, () => this.A },
                { 5, () => this.B },
                { 6, () => this.C }
            };
            this.opCodeDict = new Dictionary<int, Action>
            {
                { 0, this.Adv },
                { 1, this.Bxl },
                { 2, this.Bst },
                { 3, this.Jnz },
                { 4, this.Bxc },
                { 5, this.Out },
                { 6, this.Bdv },
                { 7, this.Cdv }
            };
            this.registers = registers;
            this.program = program;
            this.pointer = pointer;
        }

        private long A { get => this.registers[0]; set => this.registers[0] = value; }
        private long B { get => this.registers[1]; set => this.registers[1] = value; }
        private long C { get => this.registers[2]; set => this.registers[2] = value; }
        private string Output => string.Join(",", this.output);
        private int LiteralOperand => this.program[this.pointer + 1];
        private long ComboOperand => this.comboOperandDict[this.program[this.pointer + 1]]();
        private static long Pow2(long x) => (long)1 << (int)Math.Min(x, 62);

        private void MovePointer2(Action action)
        {
            action();
            this.pointer += 2;
        }

        private void Adv() => this.MovePointer2(() => this.A /= Pow2(this.ComboOperand));
        private void Bxl() => this.MovePointer2(() => this.B ^= this.LiteralOperand);
        private void Bst() => this.MovePointer2(() => this.B = this.ComboOperand % 8);
        private void Jnz() => this.pointer = this.A == 0 ? this.pointer + 2 : this.LiteralOperand;
        private void Bxc() => this.MovePointer2(() => this.B ^= this.C);
        private void Out() => this.MovePointer2(() => this.output.Add((int)(this.ComboOperand % 8)));
        private void Bdv() => this.MovePointer2(() => this.B = this.A / Pow2(this.ComboOperand));
        private void Cdv() => this.MovePointer2(() => this.C = this.A / Pow2(this.ComboOperand));

        public string Run()
        {
            while (this.pointer < this.program.Length)
            {
                this.opCodeDict[this.program[this.pointer]]();
            }

            return this.Output;
        }
    }
}
