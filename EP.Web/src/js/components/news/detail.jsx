import { h, Component } from "preact";
import activity1 from '../../../assets/images/news/1.png';
class NewsDetail extends Component {

    render() {
        return (
            <div class="jumbotron jumbotron-fluid">
                <div class="container">
                    <img class="img-fluid"
                        style={{ margin: 'auto 185px' }}
                        src={activity1} />
                </div>
            </div>
        );
    };
}

export default NewsDetail;
