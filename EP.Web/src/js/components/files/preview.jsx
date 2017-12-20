import { h, Component } from "preact";
// import { Container, Image } from 'semantic-ui-react';
import API from '../api';

class Preview extends Component {
    constructor(props) {
        super(props);
        this.state = {
            url: API.getBaseUri() + 'admin/blobManager/' + this.props.match.params.id
        }
    }

    render() {
        return (
        // <Container >
        //     <Image src={this.state.url} fluid />
        // </Container>
        <div>pewview file</div>
        );
    };
}

export default Preview;
