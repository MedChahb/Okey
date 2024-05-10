"use client";

import React from "react";
import Image from "next/image";

const OkeyHistory = () => {
    return (

        <div className="flex h-1/4">
      {/* Colonne de gauche avec une image */}
      <div className="w-1/2 h-full flex items-center justify-center">
        <img
          src="/imgs/history_pic.jpeg"
          alt="Image"
          className="h-full w-auto max-h-full max-w-full ml-8"
        />
      </div>
      {/* Colonne de droite avec du texte */}
      <div className="w-1/2 bg-gray-200 p-8">
        <p className="text-lg">Le rummikub, le pionnier du jeu de rami moderne, a été inventé par un juif d'origine roumaine nommé Ephraim Hertzano, qui a immigré en Palestine au début des années 1930.[1] Il a fabriqué son premier ensemble de jeu pour sa famille dans le jardin de sa maison. Le jeu est un mélange de rami et de mahjong.[1] C'est le jeu d'exportation numéro 1 d'Israël.[1] En 1977, il devient le jeu le plus vendu aux États-Unis.[2]

Okey semble être une évolution du Rummikub, que les travailleurs turcs « Gastarbeiter » rencontraient en Allemagne.</p>
      </div>
    </div>
    );   

};

export default OkeyHistory;