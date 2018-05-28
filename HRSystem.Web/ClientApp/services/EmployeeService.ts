import {
    IAddEmployeeParams,
    EmployeeSavingInfoResponse,
    IEmployeeService,
    GetAllEmployeesResponse, AddEmployeeParams
} from "../core/IEmployeeService";
import { IDataService } from "../core/IDataService";
import { RequestUrls } from "./RequestUrls";
import { StringHelper } from "../helpers/StringHelper";

export class EmployeeService implements IEmployeeService {
    constructor(private dataService: IDataService) {

    }

    public async getAll(): Promise<GetAllEmployeesResponse> {
        let response = await this.dataService.makeGetRequest<GetAllEmployeesResponse>(RequestUrls.GET_ALL_EMPLOYEES);
        return new GetAllEmployeesResponse(response);
    }

    public async getEmployeeSavingInfo(login: string, isCreate: boolean): Promise<EmployeeSavingInfoResponse> {
        if (login == null) {
            login = "";
        }

        let info = await this.dataService.makeGetRequest<EmployeeSavingInfoResponse>(`${RequestUrls.GET_EMPLOYEE_CREATION_INFO}?login=${login}&isCreate=${isCreate}`);
        return new EmployeeSavingInfoResponse(info);
    }

    public addNewEmployee(request: IAddEmployeeParams): Promise<void> {
        return this.dataService.makePostRequest(RequestUrls.SAVE_EMPLOYEE, new AddEmployeeParams(request));
    }
}