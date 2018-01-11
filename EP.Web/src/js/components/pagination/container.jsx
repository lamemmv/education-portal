import { h, Component } from "preact";
import { Link, HashRouter } from 'react-router-dom';
import { Localizer, Text } from 'preact-i18n';
import * as styles from './styles.css';
let classNames = require('classnames');

class EPPagination extends Component {
    render() {
        let {
            currentPage,
            pages,
            action,
            size
        } = this.props;
        if (!pages) {
            pages = [];
        }
        return (
            <ul class="pagination justify-content-end">
                <li className={classNames('page-item', { 'disabled': currentPage <= 1 })}>
                    <a role='button' class="page-link" tabindex="-1"
                        onClick={() => action({ page: currentPage - 1, size: size })}><Text id='previous'></Text></a>
                </li>
                {
                    pages.map(page => (
                        <li className={classNames('page-item', { 'active': page == currentPage })}>
                            <a class='page-link'
                                role="button"
                                onClick={() => action({ page: page, size: size })}>{page}
                                {page == currentPage ? <span class="sr-only">(current)</span> : null}
                            </a>
                        </li>
                    ))
                }
                <li className={classNames('page-item', { 'disabled': currentPage >= pages.length })}>
                    <a role='button' class="page-link"
                        onClick={() => action({ page: currentPage + 1, size: size })}><Text id='next'></Text></a>
                </li>
            </ul>
        );
    };
}

export default EPPagination;
