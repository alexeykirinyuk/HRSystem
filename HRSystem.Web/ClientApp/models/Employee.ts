import {AttributeBase} from "./AttributeBase";
import {StringHelper} from "../helpers/StringHelper";
import {IEmployee} from "../core/IEmployee";

export class Employee implements IEmployee {
    public login: string;
    public fullName: string;
    public firstName: string;
    public lastName: string;
    public email: string;
    public phone: string;
    public jobTitle: string;
    public manager: Employee;
    public attributes: AttributeBase[];
}