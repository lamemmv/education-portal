import { h, Component } from "preact";
import * as styles from './pagination.css';
let classNames = require('classnames');

class Pagination extends Component {

    render() {
        const {
            currentPage,
            pages
        } = this.props.fileState;

        const { getFiles } = this.props;

        return (
            <div class="mdc-grid-list ep-pg">
                <ul class="mdc-grid-list__tiles">
                    <li class="mdc-grid-tile"
                        className={classNames({ 'disabled': currentPage <= 1 })}>
                        <a role="button"
                            onClick={() => getFiles(currentPage - 1)}>
                            <i class="material-icons">chevron_left</i></a>
                    </li>
                    {
                        pages.map(page => (
                            <li class="mdc-grid-tile" className={classNames({ 'active': page == currentPage })}>
                                <a role="button" onClick={() => getFiles(page)}>{page}</a>
                            </li>
                        ))
                    }
                    <li class="mdc-grid-tile"
                        className={classNames({ 'disabled': currentPage >= pages.length })}>
                        <a role="button"
                            onClick={() => getFiles(currentPage + 1)}>
                            <i class="material-icons">chevron_right</i></a>
                    </li>
                </ul>
            </div>
        );
    }
}

export default Pagination;