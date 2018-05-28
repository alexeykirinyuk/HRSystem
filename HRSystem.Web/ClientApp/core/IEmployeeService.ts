import {Employee} from "../models/Employee";
import {AttributeInfo} from "../models/AttributeInfo";
import {IEmployee} from "./IEmployee";
import {AttributeType} from "../models/AttributeType";


export interface IGetAllEmployeesResponse {
    employees: Array<Employee>;
    attributes: Array<AttributeInfo>;
}

export interface IGetAllAttributesResponse {
    attributes: Array<AttributeInfo>;
}

export interface IEmployeeCreationInfoResponse {
    employees: Array<Employee>;
    attributes: Array<AttributeInfo>;
}

export interface IAddEmployeeParams {
    login: string;
    firstName: string;
    lastName: string;
    email: string;
    jobTitle: string;
    phone: string;
    managerLogin: string;
    attributes: IEmployeeAttribute[];
}

export interface IEmployeeAttribute {
    attributeInfoId: number;
    type: AttributeType;
    value: string;
}

export interface IEmployeeService {
    getAll(): Promise<IGetAllEmployeesResponse>;
    getEmployeeCreationInfo(): Promise<IEmployeeCreationInfoResponse>;
    addNewEmployee(request: IAddEmployeeParams): Promise<void>;
}