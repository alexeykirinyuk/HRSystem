import {GetAllAttributesResponse} from "./IEmployeeService";
import {AttributeType} from "../models/AttributeType";

export interface ISaveAttributeParams {
    id?: number;
    name: string;
    type: AttributeType;
}

export interface IAttributeSavingInfoQueryResponse {
    name: string;
    type: AttributeType;
}

export interface IAttributeService {
    getAll(): Promise<GetAllAttributesResponse>;

    getSavingInfo(id?: number): Promise<IAttributeSavingInfoQueryResponse>;

    save(request: ISaveAttributeParams): Promise<void>;
}