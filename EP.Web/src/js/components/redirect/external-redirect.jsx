import { h, Component } from "preact";
import API from '../api';

export class ExternalRedirect extends Component {
    componentWillMount() {
        window.location = `${API.getBaseUri()}admin/blobManager/File/${this.props.match.params.id}`;;
    }

    render() {
        return (<section></section>);
    }

}

export default ExternalRedirect;