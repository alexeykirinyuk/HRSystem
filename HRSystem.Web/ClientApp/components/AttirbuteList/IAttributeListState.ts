import {AttributeInfo} from "../../models/AttributeInfo";

export interface IAttributeListState {
    attributes: Array<AttributeInfo>;
    showModal: boolean;
    idUpdate?: number;
    isLoading: boolean;
}