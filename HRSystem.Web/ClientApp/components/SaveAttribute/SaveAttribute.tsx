import * as React from "react";
import { ISaveAttributeProps } from "./ISaveAttributeProps";
import { ISaveAttributeState } from "./ISaveAttributeState";
import { Alert, Button, ControlLabel, FormControl, FormGroup, Modal } from "react-bootstrap";
import { EventHelper } from "../../helpers/EventHelper";
import { StringHelper } from "../../helpers/StringHelper";
import { AttributeType, AttributeTypeHelper } from "../../models/AttributeType";
import Select, { Option } from "react-select";
import { Spinner } from "../Spinner/Spinner";
import { ValidationErrors } from "../../models/ValidationErrors";

export class SaveAttribute extends React.Component<ISaveAttributeProps, ISaveAttributeState> {
    public constructor(props: ISaveAttributeProps) {
        super(props);

        this.state = this.getEmptyState(props.id, props.show);
    }

    public render(): React.ReactElement<ISaveAttributeProps> {
        return (<Modal show={this.state.show} onHide={() => this.hide()}
            onEscapeKeyUp={() => this.hide()}>
            <Modal.Header>
                <Modal.Title>Create attribute</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Alert bsStyle="danger" hidden={this.state.validationErrors.length == 0}>
                    <ul>
                        {this.state.validationErrors.map(e => <li>{e.message}</li>)}
                    </ul>
                </Alert>
                <div>
                    <div hidden={!this.state.isLoading}>
                        <Spinner/>
                    </div>
                    <div hidden={this.state.isLoading}>
                        <FormGroup>
                            <ControlLabel>Name</ControlLabel>
                            <FormControl
                                type="text"
                                placeholder="Enter name"
                                value={this.state.name}
                                onChange={(event) => this.setState({name: EventHelper.getValue(event)})}/>
                        </FormGroup>
                        <FormGroup>
                            <ControlLabel>Type</ControlLabel>
                            <Select
                                value={this.state.attributeTypeOption}
                                onChange={option => this.setState({
                                    type: (option as Option<AttributeType>).value,
                                    attributeTypeOption: option
                                })}
                                options={AttributeTypeHelper.getAll().map(a => {
                                    return {label: AttributeType[a], value: a}
                                })}>
                            </Select>
                        </FormGroup>
                    </div>
                </div>
            </Modal.Body>
            <Modal.Footer>
                <Button bsStyle="danger" onClick={() => this.delete()}>DELETE</Button>
                <Button onClick={() => this.hide()}>CLOSE</Button>
                <Button bsStyle="primary"
                        onClick={() => this.clickSave()}>{this.state.id == null ? "CREATE" : "UPDATE"}</Button>
            </Modal.Footer>
        </Modal>);
    }

    public componentWillReceiveProps(props: ISaveAttributeProps) {
        if (props.show && !this.state.show) {
            this.componentDidMountAsync(props.id).then();
        }
        this.setState({ id: props.id, show: props.show });
    }

    public componentDidMount(): void {
        this.componentDidMountAsync(this.props.id).then();
    }

    private async componentDidMountAsync(id: number): Promise<void> {
        this.setState({isLoading: true});
        let getSavingInfo = await this.props.service.getSavingInfo(id);
        this.setState({
            name: getSavingInfo.name,
            type: getSavingInfo.type,
            attributeTypeOption: {label: AttributeType[getSavingInfo.type], value: getSavingInfo.type},
            isLoading: false
        });
    }

    private clickSave(): void {
        this.clickSaveAsync().then();
    }

    private async clickSaveAsync(): Promise<void> {
        try {
            await this.props.service.save(this.state);
            this.hide();
        } catch (e) {
            if (ValidationErrors.isValidationError(e)) {
                this.setState({validationErrors: ValidationErrors.parse(e)});
                console.log(e);
            } else {
                console.error(e);
            }
        }
    }

    private hide(): void {
        if (this.props.onHide != null) {
            this.props.onHide();
        }

        this.setState(this.getEmptyState(null, false));
    }

    private delete() {
        this.props.service.deleteAttribute(this.state.id)
            .then(() => {
                this.hide();
            });
    }

    private getEmptyState(id: number, show: boolean): ISaveAttributeState {
        return {
            id: id,
            show: show,
            name: StringHelper.EMPTY,
            type: AttributeType.String,
            attributeTypeOption: {
                label: AttributeType[AttributeType.String],
                value: AttributeType.String
            } as Option<AttributeType>,
            isLoading: true,
            validationErrors: []
        };
    }
}