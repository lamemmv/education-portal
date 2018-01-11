import { h, Component } from "preact";
import * as styles from './styles.css';

class EPSpinner extends Component {
    render() {
        const { loading } = this.props;
        return (
            <div class='ep-spinner'>
                {
                    loading ? <svg class="spinner" width="65px" height="65px" viewBox="0 0 66 66" xmlns="http://www.w3.org/2000/svg">
                        <circle class="path" fill="none" stroke-width="6" stroke-linecap="round" cx="33" cy="33" r="30"></circle>
                    </svg> : null
                }
            </div>
        );
    };
}

export default EPSpinner;
