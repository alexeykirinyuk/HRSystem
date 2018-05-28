import {
    IEmployeeService,
    IGetAllEmployeesResponse,
    IEmployeeCreationInfoResponse,
    IAddEmployeeParams
} from "../core/IEmployeeService";
import {IDataService} from "../core/IDataService";
import {RequestUrls} from "./RequestUrls";

export class EmployeeService implements IEmployeeService {
    constructor(private dataService: IDataService) {

    }

    public async getAll(): Promise<IGetAllEmployeesResponse> {
        return await this.dataService.makeGetRequest<IGetAllEmployeesResponse>(RequestUrls.GET_ALL_EMPLOYEES);
    }

    public getEmployeeCreationInfo(): Promise<IEmployeeCreationInfoResponse> {
        return this.dataService.makeGetRequest(RequestUrls.GET_EMPLOYEE_CREATION_INFO);
    }

    public addNewEmployee(request: IAddEmployeeParams): Promise<void> {
        return this.dataService.makePostRequest(RequestUrls.ADD_EMPLOYEE, request);
    }
}