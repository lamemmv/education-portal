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
                                <th>Title</th>
                                <th>Ingress</th>
                                <th>Content</th>
                                <th>PublishedDate</th>
                                <th>Link</th>
                            </tr>
                        </thead>
                        <tbody>
                            {items.map((item) => {
                                return (
                                    <tr>
                                        <td data-title="Title">{item.title}</td>                                        
                                        <td data-title="Ingress">{item.ingress}</td>
                                        <td data-title="Content">{item.content}</td>
                                        <td data-title="PublishedDate">{item.createdOn}</td>
                                        <td data-title="Link">
                                            <a href="https://github.com/zavoloklom/material-design-color-palette">Link</a>
                                        </td>
                                    </tr>
                                );
                            })}
                        </tbody>
                    </table>
                </div>
            </section>
        );
    }
}

export default NewsList;
