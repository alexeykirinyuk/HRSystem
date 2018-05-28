export class ActiveDirectoryAttributeInfo {
    public id: number;
    public name: string;

    public constructor(attribute?: ActiveDirectoryAttributeInfo) {
        this.id = attribute.id;
        this.name = attribute.name;
    }
}