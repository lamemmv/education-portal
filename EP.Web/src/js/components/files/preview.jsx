import { h, Component } from "preact";
import API from '../api';

class Preview extends Component {
    constructor(props) {
        super(props);
        this.state = {
            url: API.getBaseUri() + 'admin/blobManager/' + this.props.match.params.id
        }
    }

    componentWillMount() {
        API.getFile(this.props.match.params.id);
    }

    render() {
        return (
            <section class='container'><img src={this.state.url} /></section>
        );
    };
}

export default Preview;
