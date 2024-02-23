import React from "react";

export default function PriorityIcon({
  priority,
  size = 1,
}: {
  priority: number;
  size?: number;
}) {
  let indicator: string;
  switch (size) {
    case 1:
      indicator =
        "home-page-waiting-players-single-player-priorities-single-indicator";
    default:
      indicator =
        "home-page-waiting-players-single-player-priorities-single-indicator-small";
  }
  switch (priority) {
    case 1:
      return (
        <>
          <img
            src="https://uxwing.com/wp-content/themes/uxwing/download/arrow-direction/arrow-bottom-direction-red-icon.png"
            className={indicator}
          />
          <img
            src="https://uxwing.com/wp-content/themes/uxwing/download/arrow-direction/arrow-bottom-direction-red-icon.png"
            className={indicator}
          />
        </>
      );
    case 2:
      return (
        <img
          src="https://uxwing.com/wp-content/themes/uxwing/download/arrow-direction/arrow-bottom-direction-red-icon.png"
          className={indicator}
        />
      );
    case 3:
      return (
        <img
          src="https://uxwing.com/wp-content/themes/uxwing/download/user-interface/minus-icon.png"
          className={indicator}
        />
      );
    case 4:
      return (
        <img
          src="https://uxwing.com/wp-content/themes/uxwing/download/arrow-direction/arrow-top-direction-green-icon.png"
          className={indicator}
        />
      );
    case 5:
      return (
        <>
          <img
            src="https://uxwing.com/wp-content/themes/uxwing/download/arrow-direction/arrow-top-direction-green-icon.png"
            className={indicator}
          />
          <img
            src="https://uxwing.com/wp-content/themes/uxwing/download/arrow-direction/arrow-top-direction-green-icon.png"
            className={indicator}
          />
        </>
      );
    default:
      return <></>;
  }
}
