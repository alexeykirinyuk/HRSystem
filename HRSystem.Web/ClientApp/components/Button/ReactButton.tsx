import * as React from "react";
import {IReactButtonProps} from "./IReactButtonProps";
import {IReactButtonState} from "./IReactButtonState";
import {Button} from "react-bootstrap";
import {Redirect} from "react-router";

export class ReactButton extends React.Component<IReactButtonProps, IReactButtonState> {
    public constructor(props: IReactButtonProps) {
        super(props);

        this.state = {
            redirect: false,
            to: props.to,
            text: props.text
        };

        this.clickOnButton = this.clickOnButton.bind(this);
    }

    public render(): React.ReactElement<IReactButtonProps> {
        return (
          this.state.redirect ?
              <Redirect to={this.state.to} /> :
              <Button onClick={this.clickOnButton}>{this.state.text}</Button>
        );
    }

    private clickOnButton(): void {
        this.setState({redirect: true});
    }

    public componentWillReceiveProps(props: IReactButtonProps): void {
        this.setState({text: props.text, to: props.to});
    }
}