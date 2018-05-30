import {
    ISaveEmployeeParams,
    EmployeeSavingInfoResponse,
    IEmployeeService,
    GetAllEmployeesResponse, AddEmployeeParams
} from "../core/IEmployeeService";
import { IDataService } from "../core/IDataService";
import { RequestUrls } from "./RequestUrls";
import { StringHelper } from "../helpers/StringHelper";
import { AttributeType } from "../models/AttributeType";

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

    public save(request: ISaveEmployeeParams): Promise<void> {
        let documentAttributes = request.attributes.filter(a => a.type == AttributeType.Document);
        for (let attribute of documentAttributes) {
            this.upload(request.login, attribute.attributeInfoId, attribute.value);
        }

        return this.dataService.makePostRequest(RequestUrls.SAVE_EMPLOYEE, new AddEmployeeParams(request));
    }

    public isFileExists(employee: string, attributeInfoId: number) : Promise<boolean> {
        return this.dataService.makeGetRequest<boolean>(`api/file/exists?employee=${employee}&attributeInfoId=${attributeInfoId}`);
    }

    public upload(employee: string, attributeInfoId: number, file: any): void {
        let data = new FormData();
        data.append('employee', employee);
        data.append('attributeInfoId', attributeInfoId.toString());
        data.append('file', file);

        let request = new XMLHttpRequest();
        request.open('POST', "http://localhost:5000/" + RequestUrls.UPLOAD_FILE);
        request.send(data);
    }
}