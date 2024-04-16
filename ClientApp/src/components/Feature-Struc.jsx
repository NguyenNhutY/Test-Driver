import React from 'react';
import './../styles/feature.css';

const Feature = ({ title, text }) => {
  const formattedText = text.split('.').map((sentence, index) => (
    <p key={index}>{sentence.trim()}</p>
  ));

  return (
    <div className="features-container__feature">
      <div className="features-container__feature-title">
        <div />
        <h1>{title}</h1>
      </div>
      <div className="features-container_feature-text">
        {formattedText}
      </div>
    </div>
  );
};

export default Feature;
