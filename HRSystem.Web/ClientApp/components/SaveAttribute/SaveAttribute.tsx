import * as React from "react";
import { ISaveAttributeProps } from "./ISaveAttributeProps";
import { ISaveAttributeState } from "./ISaveAttributeState";
import { Button, ControlLabel, FormControl, FormGroup, Modal } from "react-bootstrap";
import { EventHelper } from "../../helpers/EventHelper";
import { StringHelper } from "../../helpers/StringHelper";
import { AttributeType, AttributeTypeHelper } from "../../models/AttributeType";
import Select, { Option } from "react-select";
import { Spinner } from "../Spinner/Spinner";

export class SaveAttribute extends React.Component<ISaveAttributeProps, ISaveAttributeState> {
    public constructor(props: ISaveAttributeProps) {
        super(props);

        this.state = {
            id: props.id,
            onHide: props.onHide,
            show: props.show,
            name: StringHelper.EMPTY,
            type: AttributeType.String,
            attributeTypeOption: {
                label: AttributeType[AttributeType.String],
                value: AttributeType.String
            } as Option<AttributeType>,
            isLoading: true
        };
    }

    public render(): React.ReactElement<ISaveAttributeProps> {
        return (<Modal show={this.state.show} onHide={this.state.onHide}>
            <Modal.Header>
                <Modal.Title>Create attribute</Modal.Title>
            </Modal.Header>
            <Modal.Body>
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
            </Modal.Body>
            <Modal.Footer>
                <Button onClick={() => this.setState({show: false})}>Close</Button>
                <Button bsStyle="primary"
                        onClick={() => this.clickSave()}>{this.state.id == null ? "Create" : "Update"}</Button>
            </Modal.Footer>
        </Modal>);
    }

    public componentWillReceiveProps(props: ISaveAttributeProps) {
        this.setState({
            id: props.id,
            show: props.show,
            onHide: props.onHide
        });

        this.componentDidMountAsync(props.id).then();
    }

    public componentDidMount(): void {
        this.componentDidMountAsync(this.state.id).then();
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
        await this.props.service.save(this.state);
        this.setState({show: false});

    }
}