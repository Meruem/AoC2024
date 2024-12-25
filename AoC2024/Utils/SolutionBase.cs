namespace AoC2024.Utils;

public abstract class SolutionBase
{
    private readonly string _baseDir = "/Users/meruem/dev/AoC2024/AoC2024/Input";
    private string[]? _lines;
    private string? _text;

    public string[] Lines
    {
        get => _lines ??= File.ReadAllLines($"{_baseDir}/{Name}.txt");
        private set => _lines = value;
    }
    
    public string InputText
    {
        get => _text ??= File.ReadAllText($"{_baseDir}/{Name}.txt");
        private set => _text = value;
    }

    public string Name => GetType().Name;
    public abstract string Part1();
    public abstract string Part2();
}