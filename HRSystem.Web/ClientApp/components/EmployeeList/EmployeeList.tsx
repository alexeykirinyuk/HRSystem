import { IEmployeeListProps } from "./IEmployeeListProps";
import { IEmployeeListState } from "./IEmployeeListState";
import * as React from "react";
import { Button, Table } from "react-bootstrap/lib";
import { Spinner } from "../Spinner/Spinner";
import { SaveEmployee } from "../SaveEmployee/SaveEmployee";
import { StringHelper } from "../../helpers/StringHelper";
import { Employee } from "../../models/Employee";

export class EmployeeList extends React.Component<IEmployeeListProps, IEmployeeListState> {
    public constructor(props: IEmployeeListProps) {
        super(props);

        this.state = {
            employees: [],
            attributes: [],
            isLoading: true,
            showModal: false,
            isCreateModalType: true,
            selectedEmployeeLogin: StringHelper.EMPTY
        };
    }

    public render(): React.ReactElement<IEmployeeListProps> {
        let attributeNameElements: JSX.Element[]
            = this.state.attributes.map(a => <th key={a.name}>{a.name}</th>);
        let employeeElements: JSX.Element[] = this.state.employees.map(e =>
            (<tr key={e.login}>
                <td>{e.fullName}</td>
                <td>{e.email}</td>
                <td>{e.phone}</td>
                <td>{e.jobTitle}</td>
                <td>{e.manager != null ? e.manager.fullName : ""}</td>
                {this.getEmployeeAttributes(e).map(a => (<td key={a.id}>{a.value}</td>))}
                <td>
                    <Button
                        onClick={() =>
                            this.setState({showModal: true, isCreateModalType: false, selectedEmployeeLogin: e.login})}>
                        Update employee</Button>
                </td>
            </tr>));

        return (<div>
            <SaveEmployee
                service={this.props.service}
                show={this.state.showModal}
                onHide={() => this.setState({showModal: false})}
                isCreate={this.state.isCreateModalType}
                login={this.state.selectedEmployeeLogin}/>
            <div>
                <Button onClick={() => this.setState({showModal: true, isCreateModalType: true})}>
                    Create employee</Button>
            </div>
            <div hidden={!this.state.isLoading}>
                <Spinner/>
            </div>
            <div hidden={this.state.isLoading}>
                <Table striped bordered condensed hover>
                    <thead>
                    <tr key="Headers">
                        <th>Name</th>
                        <th>Email</th>
                        <th>Phone</th>
                        <th>Job Title</th>
                        <th>Manager</th>
                        {attributeNameElements}
                        <th/>
                    </tr>
                    </thead>
                    <tbody>
                    {employeeElements}
                    </tbody>
                </Table>
            </div>
        </div>);
    }

    public componentDidMount(): void {
        this.setState({isLoading: true});
        this.componentDidMountAsync().then();
    }

    private async componentDidMountAsync(): Promise<void> {
        let getAllEmployeesResponse = await this.props.service.getAll();
        this.setState({
            employees: getAllEmployeesResponse.employees,
            attributes: getAllEmployeesResponse.attributes,
            isLoading: false
        });
    }

    private getEmployeeAttributes(employee: Employee): {id: number, value: string}[] {
        return this.state.attributes.map(attributeInfo => {
            let attributeBase = employee.attributes.filter(a => a.attributeInfo.id == attributeInfo.id);
            if (attributeBase.length > 0) {
                return {id: attributeInfo.id, value: attributeBase[0].value};
            } else {
                return {id: attributeInfo.id, value: StringHelper.EMPTY};
            }
        });
    }
}