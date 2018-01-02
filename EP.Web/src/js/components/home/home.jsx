import { h, Component } from 'preact';
import ShortNews from './shortNews/index';
import EPActivities from './activities/index';
import * as styles from './styles.css';

class Home extends Component {
    render() {
        return (
            <div class="mdc-layout-grid">
                <div class="mdc-layout-grid__inner">
                    <div class="mdc-layout-grid__cell mdc-layout-grid__cell--span-12">
                        <ShortNews />
                    </div>
                </div>
                <div class="mdc-layout-grid__inner" style={{ marginTop: '80px' }}>
                    <div class="mdc-layout-grid__cell mdc-layout-grid__cell--span-8">
                        <div class="mdc-card">
                            <section class="mdc-card__media">
                                <img class="mdc-card__media-item"
                                    src={require('../../../assets/images/award.jpg')} style={{ height: 'auto' }} />
                            </section>
                            <section class="mdc-card__supporting-text">
                                Giáo viên Lê Văn Khiêm vừa được phòng giáo dục Tân Phú ban tặng danh hiệu giáo viên xuất sắc, có nhiều đóng góp tích cực cho sự phát triển giáo dục quận nhà.
                            </section>
                        </div>
                    </div>
                    <div class="mdc-layout-grid__cell mdc-layout-grid__cell--span-4">
                        <EPActivities />
                    </div>
                </div>
            </div>
        );
    }
}

export default Home;