import { h, Component } from 'preact';
import * as styles from './create.css';

class CreateNews extends Component {
    render() {
        const { createNews } = this.props.actions;
        const { news } = this.props;
        return (
            <section class='ep-container'>
                <form role='form'>
                    <div class="form-group">
                        <input type="text"
                            class="form-control"
                            id="newsTitle"
                            value={news.title} />
                        <span class="form-highlight"></span>
                        <span class="form-bar"></span>
                        <label for="newsTitle">Title</label>
                    </div>
                    <div class="form-group">
                        <input type="text"
                            class="form-control"
                            id="newsIngress"
                            value={news.ingress} />
                        <span class="form-highlight"></span>
                        <span class="form-bar"></span>
                        <label for="newsIngress">Ingress</label>
                    </div>
                    <div class="form-group">
                        <textarea class="form-control"
                            rows="3"
                            id='newsContent'
                            value={news.content}></textarea>
                        <span class="form-highlight"></span>
                        <span class="form-bar"></span>
                        <label for="newsContent">Content</label>
                    </div>
                    <div class="form-group checkbox">
                        <input type="checkbox" id="newsPublished" />
                        <label for="newsPublished"><span class="chk-span" tabindex="3"></span>Published</label>
                    </div>
                    <button type="button"
                        class="mdc-button mdc-button--raised"
                        onClick={() => createNews(news)}>Create</button>
                </form>
            </section>
        );
    }
}

export default CreateNews;
