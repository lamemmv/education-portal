import { h, Component } from 'preact';
import { Link } from 'react-router-dom';
import * as styles from './list.css';

class NewsList extends Component {

    componentWillMount() {
        this.props.actions.getNews({ page: 1 });
    }

    render() {
        const { items } = this.props.news;
        const { gotoCreateNews } = this.props.actions;
        return (
            <section class='ep-container'>
                <button class="mdc-fab material-icons" aria-label="Favorite">
                    <Link to='news/create'>
                        <span class="mdc-fab__icon">
                            add
                        </span>
                    </Link>
                </button>
                <div class='shadow-z-1' style={{ marginTop: 10 }}>
                    <table class="table table-hover table-mc-light-blue">
                        <thead>
                            <tr>
                                <th>ID</th>
                                <th>Name</th>
                                <th>Link</th>
                                <th>Status</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td data-title="ID">1</td>
                                <td data-title="Name">Material Design Color Palette</td>
                                <td data-title="Link">
                                    <a href="https://github.com/zavoloklom/material-design-color-palette" target="_blank">GitHub</a>
                                </td>
                                <td data-title="Status">Completed</td>
                            </tr>
                            <tr>
                                <td data-title="ID">2</td>
                                <td data-title="Name">Material Design Iconic Font</td>
                                <td data-title="Link">
                                    <a href="https://codepen.io/zavoloklom/pen/uqCsB" target="_blank">Codepen</a>
                                    <a href="https://github.com/zavoloklom/material-design-iconic-font" target="_blank">GitHub</a>
                                </td>
                                <td data-title="Status">Completed</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </section>
        );
    }
}

export default NewsList;
