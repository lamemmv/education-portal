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
                <ol class="container breadcrumb ep-breadcrumb">
                    {
                        items.map((item, i) => {
                            return (
                                (breadcrumbsLength == (i + 1)) ?
                                    (<li class="breadcrumb-item ep-bc-item active">
                                        <i class='material-icons ep-icon'>{item.icon}</i>
                                        <Text id={item.name}>{item.name}</Text>
                                    </li>)
                                    : (<li class="breadcrumb-item ep-bc-item"><Link to={item.path}>
                                        <i class='material-icons ep-icon'>{item.icon}</i>
                                        <Text id={item.name}>{item.name}</Text>
                                    </Link></li>)
                            );
                        })
                    }
                </ol>
            </HashRouter>
        );
    };
}

export default EPBreadcrumbs;
