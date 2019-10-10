export class FileInfo {
    
    public static createInstance({ childNodes, name, isDirectory, isExpandable }) : FileInfo {
        return new FileInfo(childNodes.map(FileInfo.createInstance), name, isDirectory, isExpandable);
    }

    constructor(public childNodes: FileInfo[], public name: string, public isDirectory: boolean, public isExpandable: boolean) {}

    get formattedName(): string {
        var result = this.name.match(/(?:.*[\\])(.*)$/);
        if (result.length == 2) {
            return result[1] == "" ? result[0] : result[1]; //Если нашли совпадение, но вторая группа пустая, выходит, что перед нами строка вида C:\
        } else {
            return this.name;
        }
    }
}