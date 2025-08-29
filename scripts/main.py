import re
from datetime import datetime

import psycopg2
from psycopg2.extras import execute_values

boardId = 0

def main():
    connection, cursor = db()

    with open("./file.dat", 'r', encoding='utf-8', errors='ignore') as file:
        lines = len(file.readlines())
        file.seek(0)
        processFile(file, lines, cursor, connection)

    connection.commit()
    cursor.close()
    connection.close()


def processFile(file, lines, cursor, connection):
    global boardId
    batch = []
    batch_size = 10000
    writeFile = open("output2.txt", "w", encoding='utf-8', errors='ignore')

    keyIndex = 0
    username = ""
    subject = ""
    date = ""
    text = ""

    i = 0
    for line in file:
        i+=1
        print("Line {} / {}".format(i, lines))
        isKeyLine = keys[keyIndex] in line or i == lines

        if (not isKeyLine and keyIndex == keysLength - 1) or keyIndex == keysLength - 1:
            keyIndex = 0
            continue

        if isKeyLine:
            if keyIndex == 1: username = get_username(line)
            elif keyIndex == 2: subject = get_subject(line)
            elif keyIndex == 3: date = get_date(line)

            if keyIndex == 0 and i != 1:
                batch.append((subject, date, format_text(text), boardId))

                if len(batch) >= batch_size:
                    execute_values(
                        cursor,
                        """
                        UPDATE posts AS p
                        SET content = v.text
                        FROM (VALUES %s) AS v(subject, date, text, boardId)
                        WHERE p.title = v.subject AND p.date = v.date AND p.board_id = v.boardId
                        """,
                        batch,
                        template=None,
                        page_size=1000
                    )

                    connection.commit()
                    batch.clear()

                username = ""
                subject = ""
                date = ""
                text = ""
                global lastActiveQuote
                lastActiveQuote = 0

            keyIndex += 1
            continue

        if keyIndex != 0: continue

        if text == "" and line == "\n": continue
        text += get_line(line)

    if batch:
        execute_values(
            cursor,
            """
            UPDATE posts AS p
            SET content = v.text FROM (VALUES %s) AS v(subject
              , date
              , text
              , boardId)
            WHERE p.title = v.subject AND p.date = v.date AND p.board_id = v.boardId
            """,
            batch,
            template=None,
            page_size=1000
        )
        connection.commit()


def countArrowsInLineStart(line):
    count = 0
    for c in line:
        if c == '>': count += 1
        else: break
    return count


def format_text(text):
    text = (re.sub(r'(?<=.{60})([^\n\"])\n([^\n])', r'\1 \2', text)
              .replace("REPLACE_WITH_NEW_LINE", "\n"))

    return text


isLastLineListItem = False
lastActiveQuote = 0

def get_line(line):
    if line == " \n" or line == "\n ":
        line = "\n"

    # Handle list
    global isLastLineListItem
    isListItem = re.search(r"^(\d+[).]|[-]\s?.)", line)

    if isListItem:
        line = "REPLACE_WITH_NEW_LINE" + line
        isLastLineListItem = True
    else: isLastLineListItem = False

    return line.replace('\x00', '')


def get_username(line):
    username = line.split("From: ")[1].strip()

    if (arrowPos := username.find("<")) != -1:
        username = username[:arrowPos].strip()

    return username.replace('\x00', '')


def get_subject(line):
    subject = line.split("Subject: ")[1].strip()
    return subject.replace('\x00', '').replace("Re: ", "")


def get_date(line):
    date = line.split("Date: ")[1].strip()
    tz = date[-3:]

    if tz in tz_map:
        date = date.replace(tz, tz_map[tz])
        date = datetime.strptime(date, "%a, %d %b %Y %H:%M:%S %z")
    else:
        date = datetime.strptime(date, "%a, %d %b %Y %H:%M:%S %z")

    return date


def db():
    connection = psycopg2.connect(
        host='localhost',
        port='5432',
        user='user',
        password='pass',
        dbname='db',
    )
    cursor = connection.cursor()

    return connection, cursor


keys = ["X-Hamster-Info", "From:", "Subject:", "Date:", "Content-Transfer-Encoding:", "Lines:"]
keysLength = len(keys)

tz_map = {
    "EDT": "-0400",
    "EST": "-0500",
    "PDT": "-0700",
    "PST": "-0800",
}

quotes_map = {
    1: False,
    2: False,
    3: False,
    4: False,
    5: False,
    6: False,
    7: False,
    8: False,
    9: False,
    10: False,
}

main()
