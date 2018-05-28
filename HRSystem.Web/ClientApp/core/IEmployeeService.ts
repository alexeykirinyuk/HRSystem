import { Employee } from "../models/Employee";
import { AttributeInfo } from "../models/AttributeInfo";
import { AttributeType } from "../models/AttributeType";
import { StringHelper } from "../helpers/StringHelper";


export class GetAllEmployeesResponse {
    employees: Array<Employee>;
    attributes: Array<AttributeInfo>;

    public constructor(response?: GetAllEmployeesResponse) {
        if (response == null) {
            return;
        }

        this.employees = response.employees.map(e => new Employee(e));
        this.attributes = response.attributes.map(a => new AttributeInfo(a));
    }
}

export class GetAllAttributesResponse {
    attributes: Array<AttributeInfo>;

    public constructor(response?: GetAllAttributesResponse) {
        if (response == null) {
            return;
        }

        this.attributes = response.attributes.map(a => new AttributeInfo(a));
    }
}

export class EmployeeSavingInfoResponse {
    employee: Employee;
    employees: Array<Employee>;
    attributes: Array<AttributeInfo>;

    public constructor(response?: EmployeeSavingInfoResponse) {
        if (response == null) {
            return;
        }

        this.employee = new Employee(response.employee);
        this.employees = response.employees.map(e => new Employee(e));
        this.attributes = response.attributes.map(a => new AttributeInfo(a));
    }
}

export interface IAddEmployeeParams {
    login: string;
    firstName: string;
    lastName: string;
    email: string;
    jobTitle: string;
    office: string;
    phone: string;
    managerLogin: string;
    attributes: IEmployeeAttribute[];
    isCreateCommand: boolean;
}

export class AddEmployeeParams implements IAddEmployeeParams {
    login: string;
    firstName: string;
    lastName: string;
    email: string;
    jobTitle: string;
    office: string;
    phone: string;
    managerLogin: string;
    attributes: IEmployeeAttribute[];
    isCreateCommand: boolean;

    public constructor(params: IAddEmployeeParams) {
        this.login = params.login;
        this.firstName = params.firstName;
        this.lastName = params.lastName;
        this.isCreateCommand = params.isCreateCommand;

        if (!StringHelper.isNullOrEmpty(params.email)) {
            this.email = params.email;
        }
        if (!StringHelper.isNullOrEmpty(params.jobTitle)) {
            this.jobTitle = params.jobTitle;
        }
        if (!StringHelper.isNullOrEmpty(params.office)) {
            this.office = params.office;
        }
        if (!StringHelper.isNullOrEmpty(params.phone)) {
            this.phone = params.phone;
        }
        if (!StringHelper.isNullOrEmpty(params.managerLogin)) {
            this.managerLogin = params.managerLogin;
        }
        this.attributes = params.attributes.filter(a => !StringHelper.isNullOrEmpty(a.value));
    }
}

export interface IEmployeeAttribute {
    attributeInfoId: number;
    type: AttributeType;
    value: string;
}

export interface IEmployeeService {
    getAll(): Promise<GetAllEmployeesResponse>;

    getEmployeeSavingInfo(login: string, isCreate: boolean): Promise<EmployeeSavingInfoResponse>;

    addNewEmployee(request: IAddEmployeeParams): Promise<void>;
}