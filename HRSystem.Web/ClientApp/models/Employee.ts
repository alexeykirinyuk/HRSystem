import {AttributeBase} from "./AttributeBase";
import {IEmployee} from "../core/IEmployee";
import { StringHelper } from "../helpers/StringHelper";
import { AttributeInfo } from "./AttributeInfo";

export class Employee implements IEmployee {
    public login: string;
    public get fullName(): string {
        return `${this.firstName} ${this.lastName}`;
    }

    public firstName: string;
    public lastName: string;
    public email: string;
    public phone: string;
    public jobTitle: string;
    public office: string;
    public manager: Employee;
    public get managerLogin(): string {
        if (this.manager != null) {
            return this.manager.login;
        }

        return "";
    }
    public get managerName(): string {
        if (this.manager != null) {
            return this.manager.fullName;
        }

        return "";
    }

    public attributes: AttributeBase[];

    public constructor(employee?: Employee) {
        if (employee == null) {
            return;
        }

        this.login = employee.login;
        this.firstName = employee.firstName;
        this.lastName = employee.lastName;
        this.email = employee.email;
        this.phone = employee.phone;
        this.jobTitle = employee.jobTitle;
        this.office = employee.office;
        if (employee.manager != null) {
            this.manager = new Employee(employee.manager);
        }
        this.attributes = employee.attributes.map(a => new AttributeBase(a));
    }
}