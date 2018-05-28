import {AttributeType} from "./AttributeType";
import {ActiveDirectoryAttributeInfo} from "./ActiveDirectoryAttributeInfo";

export class AttributeInfo {
    public id: number;
    public name: string;
    public type: AttributeType;
    public activeDirectoryAttributeInfo: ActiveDirectoryAttributeInfo;
}