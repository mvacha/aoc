public class Day07 : IDay
{
    public int DayNumber => 7;
    public record File(string Name, int Size);
    public record Folder(string Name, Folder? Parent, List<File> Files, List<Folder> Folders)
    {
        private Lazy<int> _size = new (() => Files.Sum(f => f.Size) + Folders.Sum(f => f.Size));
        public int Size => _size.Value;
    }

    public Folder BuildFS(IEnumerable<string> cmdInput)
    {
        var root = new Folder("", null, new(), new());
        var currFolder = root;

        foreach (var parts in cmdInput.Select(i => i.Split(' ')))
        {
            //$ CD folderName
            if (parts[1] == "cd")
            {
                if (parts[2] == "..")
                {
                    currFolder = currFolder.Parent ?? currFolder; //handle .. from root
                }
                else if (parts[2] != "/")
                {
                    var newFolder = new Folder(parts[2], currFolder, new(), new());

                    currFolder.Folders.Add(newFolder);
                    currFolder = newFolder;
                }
            }
            //1234 file.a
            else if (int.TryParse(parts[0], out var size))
            {
                currFolder.Files.Add(new (parts[1], size));
            }
            //$ ls -> Ignored
            //dir dirName -> Ignored
        }

        return root;
    }

    static int SumFoldersWithSizeBellow(Folder f)
        => (f.Size < 100_000 ? f.Size : 0) + f.Folders.Sum(SumFoldersWithSizeBellow);   


    private static int FindSmallestFolderSizeToDelete(Folder folder, int spaceNeeded, int minFound)
    {
        if (folder.Size >= spaceNeeded && folder.Size < minFound)
            minFound = folder.Size;

        foreach (var subFolder in folder.Folders)
        {
            var minSubfolder = FindSmallestFolderSizeToDelete(subFolder, spaceNeeded, minFound);
            if (minSubfolder < minFound)
                minFound = minSubfolder;
        }

        return minFound;
    }

    public object SolveFirst(string file)
    {
        var root = BuildFS(System.IO.File.ReadAllLines(file));

        return SumFoldersWithSizeBellow(root);
    }

    public object SolveSecond(string file)
    {
        var root = BuildFS(System.IO.File.ReadAllLines(file));
        var spaceNeeded = Math.Abs(70000000 - 30000000 - root.Size);

        return FindSmallestFolderSizeToDelete(root, spaceNeeded, root.Size);
    }
}