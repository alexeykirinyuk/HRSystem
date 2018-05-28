export interface IDataService {
    makeGetRequest<T>(url: string): Promise<T>;
    makePostRequest(url: string, body: any): Promise<void>;
}