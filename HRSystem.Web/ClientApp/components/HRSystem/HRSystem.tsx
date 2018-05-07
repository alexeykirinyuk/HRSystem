import * as React from "react";
import {DefaultButton} from "office-ui-fabric-react/lib/Button"

export class HRSystem extends React.Component<IHRSystemProps, IHRSystemState> {
    public render(): React.ReactElement<IHRSystemProps> {
        return (
          <DefaultButton
              text="test"
              primary={true}
          />
        );
    }
}