import * as React from "react";
import { IAttributeListProps } from "./IAttributeListProps";
import { IAttributeListState } from "./IAttributeListState";
import { Button, Table } from "react-bootstrap";
import { SaveAttribute } from "../SaveAttribute/SaveAttribute";
import { AttributeType } from "../../models/AttributeType";
import { AttributeInfo } from "../../models/AttributeInfo";
import { Spinner } from "../Spinner/Spinner";

export class AttributeList extends React.Component<IAttributeListProps, IAttributeListState> {
    public constructor(props: IAttributeListProps) {
        super(props);

        this.state = {
            attributes: [],
            showModal: false,
            isLoading: true
        };
    }

    public render(): React.ReactElement<IAttributeListProps> {
        return (<div>
            <SaveAttribute
                id={this.state.idUpdate}
                show={this.state.showModal}
                onHide={() => this.hideModal()}
                service={this.props.service}/>
            <Button onClick={() => this.setState({showModal: true})}>Create attribute</Button>
            <div hidden={!this.state.isLoading}>
                <Spinner />
            </div>
            <div hidden={this.state.isLoading}>
                <Table striped bordered condensed hover>
                    <thead>
                    <tr>
                        <th>Name</th>
                        <th>Type</th>
                        <th/>
                    </tr>
                    </thead>
                    <tbody>
                    {
                        this.state.attributes.map(a => <tr key={a.id}>
                            <td>{a.name}</td>
                            <td>{AttributeType[a.type]}</td>
                            <td><Button bsStyle="primary" onClick={() => this.update(a)}>Update</Button></td>
                        </tr>)
                    }
                    </tbody>
                </Table>
            </div>
        </div>);
    }

    public componentDidMount(): void {
        this.componentDidMountAsync().then();
    }

    private async componentDidMountAsync(): Promise<void> {
        let response = await this.props.service.getAll();
        this.setState({attributes: response.attributes, isLoading: false});
    }

    private update(a: AttributeInfo) {
        this.setState({showModal: true, idUpdate: a.id});
    }

    private hideModal(): void {
        this.setState({showModal: false, isLoading: true, idUpdate: null});
        this.componentDidMountAsync().then();
    }
}