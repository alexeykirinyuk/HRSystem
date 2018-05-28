import {Employee} from "../models/Employee";
import {AttributeBase} from "../models/AttributeBase";

export interface IEmployee {
    login: string;
    firstName: string;
    lastName: string;
    email: string;
    phone: string;
    jobTitle: string;
    manager: Employee;
    attributes: AttributeBase[];
}