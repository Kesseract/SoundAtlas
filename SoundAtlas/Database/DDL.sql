

CREATE TABLE virtual_instruments (
    virtual_instruments_id int AUTO_INCREMENT PRIMARY KEY,
    name varchar(255) NOT NULL,
    site_url text,
    version varchar(255),
    last_updated datetime NOT NULL,
    image text,
    memo text,
);

CREATE TABLE instruments_categories (
    instrument_categories_id int AUTO_INCREMENT PRIMARY KEY,
    classification1 varchar(255) NOT NULL,
    classification2 varchar(255),
    classification3 varchar(255),
    classification4 varchar(255)
);

CREATE TABLE instruments (
    instruments_id int AUTO_INCREMENT PRIMARY KEY,
    instrument_categories_id int NOT NULL,
    name varchar(255) NOT NULL,
    FOREIGN KEY (instrument_categories_id) REFERENCES instruments_categories(instrument_categories_id)
);

CREATE TABLE virtual_instrument_presets (
    virtual_instrument_presets_id int AUTO_INCREMENT PRIMARY KEY,
    virtual_instruments_id int NOT NULL,
    instruments_id int NOT NULL,
    preset_name varchar(255) NOT NULL,
    rate int NOT NULL,
    melody_flg boolean NOT NULL,
    chord_flg boolean NOT NULL,
    bass_flg boolean NOT NULL,
    chord_rhythm_flg boolean NOT NULL,
    percussion_flg boolean NOT NULL,
    FOREIGN KEY (virtual_instruments_id) REFERENCES virtual_instruments(virtual_instruments_id),
    FOREIGN KEY (instruments_id) REFERENCES instruments(instruments_id)
);

CREATE TABLE virtual_instrument_parameters (
    virtual_instrument_parameters_id int AUTO_INCREMENT PRIMARY KEY,
    virtual_instrument_presets_id int NOT NULL,
    name varchar(255) NOT NULL,
    value varchar(255) NOT NULL,
    FOREIGN KEY (virtual_instrument_presets_id) REFERENCES virtual_instrument_presets(virtual_instrument_presets_id)
);

CREATE TABLE theories (
    theories_id int AUTO_INCREMENT PRIMARY KEY,
    name varchar(255) NOT NULL,
    melody_flg boolean NOT NULL,
    chord_flg boolean NOT NULL,
    rhythm_flg boolean NOT NULL
);

CREATE TABLE theory_details (
    theory_details_id int AUTO_INCREMENT PRIMARY KEY,
    theories_id int NOT NULL,
    memo text,
    FOREIGN KEY (theories_id) REFERENCES theories(theories_id)
);

CREATE TABLE words (
    words_id INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT NOT NULL,
    abstract TEXT,
    detail TEXT
);

CREATE TABLE theory_word_linkages (
    theory_word_linkages_id int AUTO_INCREMENT PRIMARY KEY,
    theories_id int NOT NULL,
    words_id int NOT NULL,
    FOREIGN KEY (theories_id) REFERENCES theories(theories_id),
    FOREIGN KEY (words_id) REFERENCES words(words_id)
);

CREATE TABLE instrument_word_linkages (
    instrument_word_linkages_id int AUTO_INCREMENT PRIMARY KEY,
    virtual_instrument_presets_id int NOT NULL,
    words_id int NOT NULL,
    FOREIGN KEY (virtual_instrument_presets_id) REFERENCES virtual_instrument_presets(virtual_instrument_presets_id),
    FOREIGN KEY (words_id) REFERENCES words(words_id)
);