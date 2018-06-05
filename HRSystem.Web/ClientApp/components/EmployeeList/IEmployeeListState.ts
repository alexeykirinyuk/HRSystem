import { Employee } from "../../models/Employee";
import { AttributeInfo } from "../../models/AttributeInfo";
import {Option} from "react-select";

export interface IEmployeeListState {
    employees: Array<Employee>;
    attributes: Array<AttributeInfo>;
    isLoading: boolean;
    showModal: boolean;
    isCreateModalType: boolean;
    selectedEmployeeLogin: string;
    search: string;

    searchManagerSelected: string;
    searchManagerSelectedOption: Option<string>;
    searchManagers: string[];

    searchOfficeSelected: string;
    searchOfficeSelectedOption: Option<string>;
    searchOffices: string[];

    searchJobTitleSelected: string;
    searchJobTitleSelectedOption: Option<string>;
    searchJobTitles: string[];

    searchAttributeValues: Map<number, string>;
}