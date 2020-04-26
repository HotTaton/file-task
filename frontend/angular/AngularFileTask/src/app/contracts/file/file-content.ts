export class FileContent {
    public static createInstance({ content }) : FileContent {
        return new FileContent(content);
    }

    constructor(public content: string[][]) { }

    public get header() : string[] {
        if (this.content) {
            return this.content[0]; //шапка всегда в 0 строке
        }        
        return null;
    }

    public get data() : string[][] {
        if (this.content) {
            return this.content.slice(1); //пропускаем 1 строку
        }
        return null;
    }
}
