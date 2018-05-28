import {IEmployeeListProps} from "./IEmployeeListProps";
import {IEmployeeListState} from "./IEmployeeListState";
import * as React from "react";
import {Button, Table} from "react-bootstrap/lib";
import {Spinner} from "../Spinner/Spinner";
import {AddEmployee} from "../AddEmployee/AddEmployee";
import {StringHelper} from "../../helpers/StringHelper";

export class EmployeeList extends React.Component<IEmployeeListProps, IEmployeeListState> {
    public constructor(props: IEmployeeListProps) {
        super(props);

        this.state = {
            employees: [],
            attributes: [],
            isLoading: true,
            showModal: false
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
                {e.attributes.map(a => (<td>{a.GetValue()}</td>))}
            </tr>));

        return (<div>
            <AddEmployee
                service={this.props.service}
                show={this.state.showModal}
                onHide={() => this.setState({showModal: false})}/>
            <div>
                <Button onClick={() => this.setState({showModal: true})}>Create employee</Button>
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
}