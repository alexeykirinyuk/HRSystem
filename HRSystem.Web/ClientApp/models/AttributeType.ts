export enum AttributeType {
    Int,
    String,
    DateTime,
    Employee,
    Bool,
    Document
}

export class AttributeTypeHelper {
    public static getAll(): Array<AttributeType> {
        return [
            AttributeType.String,
            AttributeType.DateTime,
            AttributeType.Int,
            AttributeType.Employee,
            AttributeType.Bool,
            AttributeType.Document
        ];
    }
}