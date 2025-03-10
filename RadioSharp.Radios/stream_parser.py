import json
from bs4 import BeautifulSoup

def read_html_from_file(file_path):
    with open(file_path, 'r', encoding='utf-8') as file:
        html_content = file.read()
    return html_content

def main():
    file_path = 'source.html'
    html_content = read_html_from_file(file_path)

    soup = BeautifulSoup(html_content, 'html.parser')

    radio_blocks = soup.find_all('div', class_='stnblock')

    radios = []
    for block in radio_blocks:
        name = block.find('h3').text.strip()

        streams = []
        for link in block.find_all('div', class_='sq'):
            title = link.get('title')
            if title:
                streams.append(title)

        if streams:
            radios.append({'name': name, 'streams': streams})

    print(json.dumps(radios, indent=4))

if __name__ == "__main__":
    main()
