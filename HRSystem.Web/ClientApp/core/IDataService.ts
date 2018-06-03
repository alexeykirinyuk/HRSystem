import { AxiosRequestConfig } from "axios";

export interface IDataService {
    makeGetRequest<T>(url: string): Promise<T>;
    makePostRequest(url: string, body: any, config?: AxiosRequestConfig): Promise<void>;
    makeDeleteRequest<T>(url: string): Promise<T>;
}