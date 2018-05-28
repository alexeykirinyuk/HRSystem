import { ISaveEmployeeProps } from "./ISaveEmployeeProps";
import { ISaveEmployeeState } from "./ISaveEmployeeState";
import * as React from "react";
import { Button, ControlLabel, FormControl, FormGroup, Modal } from "react-bootstrap";
import { AttributeType } from "../../models/AttributeType";
import { AttributeInfo } from "../../models/AttributeInfo";
import { StringHelper } from "../../helpers/StringHelper";
import { EventHelper } from "../../helpers/EventHelper";
import Select, { Option } from "react-select";

export class SaveEmployee extends React.Component<ISaveEmployeeProps, ISaveEmployeeState> {
    private static readonly typeMap: Map<AttributeType, string> =
        new Map<AttributeType, string>();

    static _initialize(): void {
        this.typeMap.set(AttributeType.Int, "number");
        this.typeMap.set(AttributeType.String, "text");
        this.typeMap.set(AttributeType.DateTime, "date");
        this.typeMap.set(AttributeType.Bool, "date");
    }

    public constructor(props: ISaveEmployeeProps) {
        super(props);

        this.state = {
            attributes: [],
            firstName: StringHelper.EMPTY,
            email: StringHelper.EMPTY,
            jobTitle: StringHelper.EMPTY,
            lastName: StringHelper.EMPTY,
            login: StringHelper.EMPTY,
            phone: StringHelper.EMPTY,
            attributesInfo: [],
            isLoading: true,
            employees: [],
            show: props.show,
            onHide: props.onHide,
            managerLoginOption: null,
            managerLogin: null,
            isCreateCommand: props.isCreate,
            office: StringHelper.EMPTY
        };
    }

    public render(): React.ReactElement<ISaveEmployeeProps> {
        let managerOptions: Option<string>[] = this.state.employees.map(e => {
            return {
                label: e.fullName,
                value: e.login
            }
        });
        managerOptions.unshift({label: "", value: ""});

        return (
            <Modal show={this.state.show} onHide={() => this.onHide()}
                   onEscapeKeyUp={() => this.setState({show: false})}>
                <Modal.Header>Create new employee</Modal.Header>
                <Modal.Body>
                    <form>
                        <FormGroup>
                            <ControlLabel>Login</ControlLabel>
                            <FormControl
                                type="text"
                                value={this.state.login}
                                disabled={!this.state.isCreateCommand}
                                placeholder="Enter login"
                                onChange={(event) => this.setState({login: EventHelper.getValue(event)})}/>
                        </FormGroup>
                        <FormGroup>
                            <ControlLabel>First name</ControlLabel>
                            <FormControl
                                type="text"
                                value={this.state.firstName}
                                placeholder="Enter first name"
                                onChange={(event) => this.setState({firstName: EventHelper.getValue(event)})}/>
                        </FormGroup>
                        <FormGroup>
                            <ControlLabel>Last name</ControlLabel>
                            <FormControl
                                type="text"
                                value={this.state.lastName}
                                placeholder="Enter last name"
                                onChange={(event) => this.setState({lastName: EventHelper.getValue(event)})}/>
                        </FormGroup>
                        <FormGroup>
                            <ControlLabel>Email</ControlLabel>
                            <FormControl
                                type="text"
                                value={this.state.email}
                                placeholder="Enter email address"
                                onChange={(event) => this.setState({email: EventHelper.getValue(event)})}/>
                        </FormGroup>
                        <FormGroup>
                            <ControlLabel>Manager</ControlLabel>
                            <Select
                                value={this.state.managerLoginOption}
                                onChange={option => this.setState({
                                    managerLogin: option == null ? managerOptions[0].value : (option as Option<string>).value,
                                    managerLoginOption: option == null ? managerOptions[0] : option
                                })}
                                options={managerOptions}>
                            </Select>
                        </FormGroup>
                        <FormGroup>
                            <ControlLabel>Job Title</ControlLabel>
                            <FormControl
                                type="text"
                                placeholder="Enter job title"
                                value={this.state.jobTitle}
                                onChange={(event) => this.setState({jobTitle: EventHelper.getValue(event)})}/>
                        </FormGroup>
                        <FormGroup>
                            <ControlLabel>Office</ControlLabel>
                            <FormControl
                                type="text"
                                placeholder="Enter office"
                                value={this.state.office}
                                onChange={(event) => this.setState({office: EventHelper.getValue(event)})}/>
                        </FormGroup>
                        <FormGroup>
                            <ControlLabel>Phone</ControlLabel>
                            <FormControl
                                type="text"
                                placeholder="Enter phone"
                                value={this.state.phone}
                                onChange={(event) => this.setState({phone: EventHelper.getValue(event)})}/>
                        </FormGroup>
                        {
                            this.state.attributesInfo.map(a => <FormGroup key={a.id}>
                                <ControlLabel>{a.name}</ControlLabel>
                                <FormControl
                                    type={SaveEmployee.typeMap.get(a.type)}
                                    placeholder={`Enter ${a.name} (custom)`}
                                    value={this.getAttribute(a)}
                                    onChange={(event) => this.changeAttribute(a, EventHelper.getValue(event))}/>
                            </FormGroup>)
                        }
                    </form>
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={() => this.setState({show: false})}>Close</Button>
                    <Button bsStyle="primary"
                            onClick={() => this.clickSaveButton()}>{this.state.isCreateCommand ? "Create" : "Update"}</Button>
                </Modal.Footer>
            </Modal>);
    }

    private getAttribute(a: AttributeInfo): any {
        let result = this.state.attributes.filter(at => at.attributeInfoId == a.id);
        if (result.length != 0) {
            return result[0].value;
        }

        return StringHelper.EMPTY;
    }

    public componentWillReceiveProps(props: ISaveEmployeeProps): void {
        if (props.show && !this.state.show) {
            this.setState({show: props.show, onHide: props.onHide, isCreateCommand: props.isCreate});
            this.componentWillMountAsync(props.login, props.isCreate).then();
        }
    }

    private async componentWillMountAsync(login: string, isCreate: boolean): Promise<void> {
        this.setState({isLoading: true});
        let info = await this.props.service.getEmployeeSavingInfo(login, isCreate);
        console.log(info.employee.managerLogin);
        console.log(info.employee.managerName);

        this.setState({
            attributesInfo: info.attributes,
            isLoading: false,
            employees: info.employees,

            login: info.employee.login,
            firstName: info.employee.firstName,
            lastName: info.employee.lastName,
            email: info.employee.email,
            phone: info.employee.phone,
            jobTitle: info.employee.jobTitle,
            office: info.employee.office,
            managerLogin: info.employee.managerLogin,
            managerLoginOption: {label: info.employee.managerName, value: info.employee.managerLogin},
            attributes: info.employee.attributes.map(a => {
                return {attributeInfoId: a.attributeInfo.id, type: a.attributeInfo.type, value: a.value}
            })
        });
    }

    private changeAttribute(attributeInfo: AttributeInfo, value: string): void {
        this.setState(state => {
            let resultAttributes = state.attributes.slice();
            let filteredById = resultAttributes.filter(a => a.attributeInfoId == attributeInfo.id);
            if (filteredById.length == 0) {
                resultAttributes.push({
                    attributeInfoId: attributeInfo.id,
                    type: attributeInfo.type,
                    value: value
                });
            } else {
                filteredById[0].value = value;
            }

            return {attributes: resultAttributes};
        });
    }

    private clickSaveButton(): void {
        this.clickSaveButtonAsync().then();
    }

    private async clickSaveButtonAsync(): Promise<void> {
        try {
            await this.props.service.addNewEmployee(this.state);
            this.setState({show: false});
        } catch (e) {
            console.log(e);
        }
    }

    private onHide() {
        if (this.state.onHide != null) {
            this.state.onHide();
        }
    }
}

SaveEmployee._initialize();