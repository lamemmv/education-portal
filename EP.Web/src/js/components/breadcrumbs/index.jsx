import { h, Component } from 'preact';
import { Link, HashRouter } from 'react-router-dom';
import { Text } from 'preact-i18n';

import * as styles from './breadcrumbs.css';

class EPBreadcrumbs extends Component {
    render() {
        const { items } = this.props.breadcrumbs;
        const breadcrumbsLength = items.length;
        return (
            <HashRouter>
                <div class="breadcrumb">
                    {
                        items.map((item, i) => {
                            return (
                                (breadcrumbsLength == (i + 1)) ?
                                    (<span class="breadcrumb__step breadcrumb__step--active">
                                        <i class='material-icons ep-icon'>{item.icon}</i>
                                        <Text id={item.name}></Text>
                                    </span>)
                                    : (<Link class="breadcrumb__step breadcrumb__step" to={item.path}>
                                        <i class='material-icons ep-icon'>{item.icon}</i>
                                        <Text id={item.name}></Text>
                                    </Link>)
                            );
                        })
                    }
                </div>
            </HashRouter>
        );
    };
}

export default EPBreadcrumbs;
