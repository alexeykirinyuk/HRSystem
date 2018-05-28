export enum AttributeType {
    Int,
    String,
    DateTime,
    Employee
}

export class AttributeTypeHelper {
    public static getAll(): Array<AttributeType> {
        return [
            AttributeType.String,
            AttributeType.DateTime,
            AttributeType.Int,
            AttributeType.Employee
        ];
    }
}