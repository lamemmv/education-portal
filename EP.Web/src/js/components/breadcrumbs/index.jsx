import { h, Component } from 'preact';
import * as styles from './breadcrumbs.css';
import { Link, HashRouter } from 'react-router-dom';

class EPBreadcrumbs extends Component {
    render() {
        const { items } = this.props.breadcrumbs;
        const breadcrumbsLength = items.length;
        return (
            <HashRouter>
                <div class="breadcrumb">
                    {
                        items.map((item, i) => {
                            return (breadcrumbsLength == i + 1) ?
                                (<span class="breadcrumb__step breadcrumb__step--active">{item.name}</span>)
                                : (<Link class="breadcrumb__step breadcrumb__step--active" to={item.path}>{item.name}</Link>);
                        })
                    }
                </div>
            </HashRouter>
        );
    };
}

export default EPBreadcrumbs;
