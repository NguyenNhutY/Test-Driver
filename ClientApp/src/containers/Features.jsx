import React from 'react';
import Feature from './../components/Feature';
import './../styles/features.css';

const featuresData = [
  {
    title: '',
        text: 'Bước 1: Các bạn cần đăng nhập vào: https://gplxweb.azurewebsites.net/',
  },
  {
    title: '',
    text: 'Bước 2: Đọc kĩ hướng dẫn, cấu trúc đề và kéo đến phần “Thi Lý Thuyết ”',
  },
  {
    title: '',
    text: 'Bước 3: Bắt đầu làm bằng cách click vào câu để chọn câu trả lời mà bạn cho là đúng.',
  },
  {
    title: '',
    text: 'Bước 4: Click “Tiếp theo” để sang câu hỏi kế tiếp',
  },
  {
    title: '',
    text: 'Bước 5: Click “HOÀN THÀNH” để nộp bài thi và kéo xuống để xem kết quả.',
  },
];

const Features = () => (
  <div className="features section__padding" id="features">
    <div className="features-heading">
      <h1 className="">Hướng dẫn làm bài thi online</h1>
      <p><b>Lưu ý:</b> Mỗi câu hỏi sẽ chỉ có 1 đáp án đúng duy nhất nên chỉ cần chọn 1 đáp án.</p>
    </div>
    <div className="features-container">
      {featuresData.map((item) => (
        <Feature title={item.title} text={item.text} key={item.title} />
      ))}
    </div>
  </div>
);

export default Features;
