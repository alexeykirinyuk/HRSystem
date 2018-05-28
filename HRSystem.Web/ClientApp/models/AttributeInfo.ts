import {AttributeType} from "./AttributeType";
import {ActiveDirectoryAttributeInfo} from "./ActiveDirectoryAttributeInfo";

export class AttributeInfo {
    public id: number;
    public name: string;
    public type: AttributeType;
    public activeDirectoryAttributeInfo: ActiveDirectoryAttributeInfo;

    public constructor(attribute?: AttributeInfo) {
        if (attribute == null) {
            return;
        }

        this.id = attribute.id;
        this.name = attribute.name;
        this.type = attribute.type;
        if (attribute.activeDirectoryAttributeInfo != null) {
            this.activeDirectoryAttributeInfo = new ActiveDirectoryAttributeInfo(attribute.activeDirectoryAttributeInfo);
        }
    }
}