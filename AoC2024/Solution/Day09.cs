namespace AoC2024.Solution;

public static class Day09
{
    public static long Part1()
    {
        var input = File.ReadAllText("Input/day09.txt");
        var elements = GetElements(input);

        var freeSpaceIndex = FindFreeSpaceIndex(elements);
        while (freeSpaceIndex != -1)
        {
            var free = elements[freeSpaceIndex];

            var availableIndex = FindLastAvailableIndex(elements);
            if (availableIndex == -1 || availableIndex < freeSpaceIndex) break;
            var available = elements[availableIndex];
            if (available.Count >= free.Count)
            {
                available.Count -= free.Count;
                free.Id = available.Id;
            }
            else
            {
                elements.Insert(freeSpaceIndex + 1, new Element { Id = null, Count = free.Count - available.Count });

                free.Count = available.Count;
                free.Id = available.Id;


                available.Count = 0;
                available.Id = null;
            }

            freeSpaceIndex = FindFreeSpaceIndex(elements);
        }

        return CheckSum(elements);
    }

    public static long Part2()
    {
        var input = File.ReadAllText("Input/day09.txt");
        // var input = "2333133121414131402";
        var elements = GetElements(input);

        var idToMove = elements.Last().Id is not null ? elements.Last().Id!.Value : elements[^2].Id!.Value;
        while (idToMove >= 0)
        {
            var elToMoveIdx = FindAvailable(elements, idToMove);
            var elToMove = elements[elToMoveIdx];
            var freeIdx = FindFreeOfSize(elements, elToMove.Count);
            if (freeIdx > -1 && freeIdx < elToMoveIdx)
            {
                var free = elements[freeIdx];
                var movedSize = elToMove.Count;
                elToMove.Id = null;
                if (elToMoveIdx < elements.Count - 1 && elements[elToMoveIdx + 1].Id is null)
                {
                    elToMove.Count += elements[elToMoveIdx + 1].Count;
                    elements.RemoveAt(elToMoveIdx + 1);
                }

                if (elements[elToMoveIdx - 1].Id is null)
                {
                    elements[elToMoveIdx - 1].Count += elToMove.Count;
                    elements.RemoveAt(elToMoveIdx);
                }

                if (free.Count > movedSize)
                    elements.Insert(freeIdx + 1, new Element { Id = null, Count = free.Count - movedSize });

                free.Count = movedSize;
                free.Id = idToMove;
            }

            idToMove--;
        }

        return CheckSum(elements);
    }

    private static long CheckSum(List<Element> elements)
    {
        var elements2 = elements.Where(el => el is { Count: > 0 }).ToList();
        long sum = 0;
        long index = 0;
        foreach (var el in elements2)
        {
            for (int i = 0; i < el.Count; i++)
            {
                if (el.Id is not null)
                    sum += index * el.Id.Value;
                index++;
            }
        }

        return sum;
    }

    private static List<Element> GetElements(string input)
    {
        var elements = new List<Element>();
        for (int i = 0; i < input.Length; i++)
        {
            elements.Add(i % 2 == 0
                ? new Element { Id = i / 2, Count = int.Parse(input[i].ToString()) }
                : new Element { Id = null, Count = int.Parse(input[i].ToString()) });
        }

        return elements;
    }

    private static int FindFreeSpaceIndex(List<Element> elements)
    {
        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].Id is null && elements[i].Count > 0) return i;
        }

        return -1;
    }

    private static int FindLastAvailableIndex(List<Element> elements)
    {
        for (int i = elements.Count - 1; i >= 0; i--)
        {
            if (elements[i].Id is not null && elements.Count > 0) return i;
        }

        return -1;
    }

    private static int FindAvailable(List<Element> elements, int id)
    {
        for (int i = elements.Count - 1; i >= 0; i--)
        {
            if (elements[i].Id == id) return i;
        }

        return -1;
    }

    private static int FindFreeOfSize(List<Element> elements, int size)
    {
        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].Id == null && elements[i].Count >= size) return i;
        }

        return -1;
    }
}

public class Element
{
    public int? Id { get; set; }
    public int Count { get; set; }
}